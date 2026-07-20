using AspNetCoreGeneratedDocument;
using Banking_Platform_2._0.Models;
using Banking_Platform_2._0.Models.DTO_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Banking_Platform_2._0.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly BankingDbContext db;
        public AdminController(BankingDbContext _db)
        {
            db = _db;
        }
        [HttpGet("home")]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet("deposit-money")]
        public IActionResult Deposit()
        {
            DepositeDTO tr = new DepositeDTO();
            return PartialView("_Deposite", tr);
        }
        [HttpPost("deposit-money")]
        public IActionResult Deposit(DepositeDTO depo)
        {
            try
            {
                var account = db.AccountMsts.FirstOrDefault(a => a.AccountNo == depo.DepositeAccount);
                if(account == null)
                {
                    return NotFound(new { message = "Account not found" });
                }
                var transaction = new Transaction
                {
                    ToAcId = account.AccId,
                    Amount = depo.Amount,
                    Type = "Deposit",
                    Timestamp = DateTime.Now
                };
                db.Transactions.Add(transaction);
                account.Balance += depo.Amount;
                db.SaveChanges();
                return Ok(new { message = "Deposit of ₹" + depo.Amount + " was successful!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred: " + ex.Message });
            }
        }
        [HttpGet("withdraw")]
        public IActionResult Withdraw()
        {
            WithdrwalDTO wd = new WithdrwalDTO();
            return PartialView("_Withdraw", wd);
        }
        [HttpPost("withdraw-money")]
        public IActionResult Withdraw(WithdrwalDTO wd)
        {
            try
            {
                var account = db.AccountMsts.FirstOrDefault(a => a.AccountNo == wd.WidtdrawAccountNo);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found" });
                }
                if(account.Balance>wd.Amount&&wd.Amount>0)
                {
                    var transaction = new Transaction
                    {
                        FromAcId = account.AccId,
                        Amount = wd.Amount,
                        Type = "Withdrawal",
                        Timestamp = DateTime.Now
                    };
                    db.Transactions.Add(transaction);
                    account.Balance -= wd.Amount;
                    db.SaveChanges();
                }
                return Ok(new { message = "Withdraw of ₹" + wd.Amount + " was successful!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred: " + ex.Message });
            }
        }
        [HttpGet("transfer")]
        public IActionResult Transfer()
        {
            TransactionDTO tr = new TransactionDTO();
            return PartialView("_Transaction", tr);
        }
        [HttpPost("transfer")]
        public IActionResult Transfer(TransactionDTO tr)
        {
            try
            {
                Transaction newTrans = new Transaction()
                {
                    FromAcId = tr.FromAcId,
                    ToAcId = tr.ToAcId,
                    Amount = tr.Amount,
                    Type = tr.TransferMode,
                    Timestamp = DateTime.Now
                };

                var subSender = db.AccountMsts.Where(a => a.AccId == tr.FromAcId).FirstOrDefault();
                var addReceiver = db.AccountMsts.Where(a => a.AccId == tr.ToAcId).FirstOrDefault();
                if (subSender.Balance > tr.Amount)
                {
                    subSender.Balance -= tr.Amount;
                    addReceiver.Balance += tr.Amount;
                }

                db.Transactions.Add(newTrans);

                db.SaveChanges();

                return PartialView("_Transaction", tr);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error : " + ex.Message, error = ex.Message });
            }
        }
        [HttpGet("create-account")]
        public IActionResult New_Account()
        {
            ViewBag.States = GetStates();
            ViewBag.Countries = GetCountries();
            ViewBag.Branch = GetBranch();
            NewAccountDTO tr = new NewAccountDTO();
            tr.DateOfBirth = DateTime.Now;
            return PartialView("_NewAccount", tr);
        }
        [HttpPost("create-account")]
        public IActionResult New_Account(NewAccountDTO ac)
        {
            if (InsertIntoUser(ac) > 0)
            {
                int UId = db.Users.Where(u => u.Email == ac.Email && u.PhoneNo == ac.MobileNumber.ToString()).FirstOrDefault().UserId;
                long accNo = InsertIntoAcc(ac, UId);
                if (accNo > 0)
                {
                    InsertIntoCustomer(ac, UId);
                    var acDetails = new CreatedAccountDto()
                    {
                        AccountNumber = accNo,
                        HolderName = ac.Name,
                        Balance = Convert.ToDecimal(ac.InitialDeposit)
                    };
                    return PartialView("_AccountDetails", acDetails);
                }
            }

            ViewBag.States = GetStates();
            ViewBag.Countries = GetCountries();
            ViewBag.Branch = GetBranch();
            return PartialView("_AccountDetails", ac);
        }
        private long InsertIntoAcc(NewAccountDTO ac, int uid)
        {
            AccountMst account = new AccountMst()
            {
                AccType = ac.AccountType,
                Balance = Convert.ToDecimal(ac.InitialDeposit),
                BranchId = db.Branches.Where(b => b.BranchCode == ac.BranchCode.ToString()).FirstOrDefault().BranchId,
                Status = true,
                AccountNo = CreatAccNo(),
                CreatedDt = DateOnly.FromDateTime(DateTime.Now)
            };
            db.AccountMsts.Add(account);
            int x=db.SaveChanges();
            if(x==0)
            {
                return 0;
            }
            return account.AccountNo;
        }
        private int InsertIntoUser(NewAccountDTO ac)
        {
            User user = new User()
            {
                Email = ac.Email,
                PhoneNo = ac.MobileNumber.ToString(),
                RoleId = 3,
                Status = false,
                Username = CreateDefaultUserPass(ac.Name, ac.DateOfBirth),
                Password = CreateDefaultUserPass(ac.Name, ac.DateOfBirth)
            };
            db.Users.Add(user);
            return db.SaveChanges();
        }
        private void InsertIntoCustomer(NewAccountDTO ac, int uid)
        {
            Customer cust = new Customer()
            {
                Name = ac.Name,
                PhoneNo = ac.MobileNumber.ToString(),
                Dob = ac.DateOfBirth,
                Gender = ac.Gender,
                FatherName = ac.FatherName,
                MotherName = ac.MotherName,
                Address = ac.Address,
                City = ac.State,
                Country = ac.Country,
                Role = 3,
                Aadhar = ac.AdharNumber,
                Pancard = ac.PanNumber,
                UserId = uid,
                //Doo = DateTime.Now.Date,
                AccId = db.AccountMsts.OrderByDescending(a => a.AccId).FirstOrDefault().AccId
            };
        }
        private long CreatAccNo()
        {
            // Generate Account Number
            var ac = db.AccountMsts.OrderByDescending(a => a.AccountNo).FirstOrDefault();
            if (ac != null)
            {
                return Convert.ToInt64(ac.AccountNo) + 1;
            }
            else
            {
                return 1000000000001000;
            }
        }
        private string CreateDefaultUserPass(string name, DateTime dateOfBirth)
        {
            // Create a default password using the first Four letters of the name and the date of birth
            string firstThreeLetters = name.Length >= 4 ? name.Substring(0, 4).ToUpper() : name.ToUpper();
            string dobPart = dateOfBirth.ToString("ddMMyyyy");
            return $"@{firstThreeLetters}{dobPart}";
        }
        [HttpGet("get-ifsc")]
        public JsonResult GetIfscByBranchId(string branchId)
        {
            var ifscCode = db.Branches
                .Where(b => b.BranchCode == branchId)
                .Select(b => b.Ifsccode)
                .FirstOrDefault();

            return Json(new { ifsc = ifscCode, branchId });
        }
        [HttpGet("modify-account")]
        public IActionResult Modify()
        {
            ViewBag.States = GetStates();
            ViewBag.Countries = GetCountries();
            ViewBag.Branch = GetBranch();
            ModifyAccountDto ac = new ModifyAccountDto();
            return PartialView("_Modify", ac);
        }
        [HttpPost("modify-account")]
        public IActionResult Modify(ModifyAccountDto ac)
        {
            AccountMst accountDetails =db.AccountMsts.Where(a => a.AccountNo == Convert.ToInt64(ac.AccountNumber)).FirstOrDefault();
            if (accountDetails != null)
            {
                accountDetails.AccType = ac.AccountType;
                accountDetails.BranchId = db.Branches.Where(b => b.BranchCode == ac.BranchCode).FirstOrDefault().BranchId;
                accountDetails.Branch = db.Branches.Where(b => b.BranchCode == ac.BranchCode).FirstOrDefault();
                db.SaveChanges();
            }
            var customerDetails = db.Customers.Where(c => c.AccId == accountDetails.AccId).FirstOrDefault();
            if (customerDetails != null)
            {
                customerDetails.Name = ac.Name;
                customerDetails.Gender = ac.Gender;
                customerDetails.FatherName = ac.FatherName;
                customerDetails.MotherName = ac.MotherName;
                customerDetails.Country = ac.Country;
                customerDetails.City = ac.City;
                db.SaveChanges();
            }
            var userDetails = db.Users.Where(u => u.AccId == accountDetails.AccId).FirstOrDefault();
            if (userDetails != null)
            {
                userDetails.Email = ac.Email;
                userDetails.PhoneNo = ac.MobileNumber.ToString();
                db.SaveChanges();
            }
            ViewBag.States = GetStates();
            ViewBag.Countries = GetCountries();
            ViewBag.Branch = GetBranch();
            ac = null;
            return StatusCode(200);
        }
        [HttpGet("verify-withdraw-acc")]
        public IActionResult WithdrawAccount(long accountNumber)
        {
            var details = (from c in db.Customers
                           join a in db.AccountMsts on c.AccId equals a.AccId
                           join u in db.Users on c.AccId equals u.AccId
                           join b in db.Branches on a.BranchId equals b.BranchId
                           where a.AccountNo == accountNumber
                           select new
                           {
                               a.AccountNo,
                               c.Name,
                               c.Gender,
                               c.PhoneNo,
                               u.Email,
                               c.FatherName,
                               c.MotherName,
                               c.Address,
                               c.City,
                               c.Country,
                               b.BranchName,
                               b.BranchCode,
                               b.Ifsccode,
                               a.AccType,
                               isActive = a.Status
                           }).ToList();


            if (details.Count() > 0)
            {
                return Ok(details);
            }
            return NotFound( new { message = "Account not found." });

        }
        [HttpGet("verify-transaction-account")]
        public IActionResult VerifySenderAccount([FromQuery] long fromAcc, [FromQuery] long toAcc)
        {
            try
            {
                var targetAccounts = new[] { fromAcc, toAcc };

                var accounts = (from a in db.AccountMsts
                                join c in db.Customers on a.AccId equals c.AccId
                                where targetAccounts.Contains(a.AccountNo)
                                select new
                                {
                                    AccountNo = a.AccountNo,
                                    AccId = a.AccId,
                                    Name = c.Name,
                                    Balance = a.Balance,
                                    AccType=a.AccType
                                }).ToList();

                var senderData = accounts.FirstOrDefault(a => a.AccountNo == fromAcc);
                var receiverData = accounts.FirstOrDefault(a => a.AccountNo == toAcc);

                if (senderData == null || receiverData == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = senderData == null ? "Sender account not found." : "Receiver account not found."
                    });
                }

                var response = new
                {
                    sender = new
                    {
                        FromAccNo = senderData.AccountNo,
                        FromAccid = senderData.AccId,
                        FromName = senderData.Name,
                        AvailableBalance = senderData.Balance
                    },
                    receiver = new
                    {
                        ToAccNo = receiverData.AccountNo,
                        ToAccid = receiverData.AccId,
                        ToName = receiverData.Name,
                        ToAccType=receiverData.AccType
                    }
                };

                return Ok(new { success = true, data = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while verifying accounts." });
            }
        }
        public IActionResult GetAccountDetails(long accountNumber)
        {
            var details = (from c in db.Customers
                          join a in db.AccountMsts on c.AccId equals a.AccId
                          join u in db.Users on c.AccId equals u.AccId
                          join b in db.Branches on a.BranchId equals b.BranchId
                          where a.AccountNo == accountNumber
                          select new
                          {
                              a.AccountNo,
                              c.Name,
                              c.Gender,
                              c.PhoneNo,
                              u.Email,
                              c.FatherName,
                              c.MotherName,
                              c.Address,
                              c.City,
                              c.Country,
                              b.BranchName,
                              b.BranchCode,
                              b.Ifsccode,
                              a.AccType,
                              isActive=a.Status
                          }).ToList();


            if (details .Count()>0)
            {
                return Json(new { success = true, data = details });
            }
            return Json(new { success = false, message = "Account not found." });

        }
        [HttpGet("verify-account")]
        public IActionResult VerifyAccount(long accountNumber)
        {
            var details = (from c in db.Customers
                          join a in db.AccountMsts on c.AccId equals a.AccId
                          where a.AccountNo == accountNumber
                          select new
                          {
                              a.AccountNo,
                              a.Balance,
                              c.Name,
                              a.AccType,
                              isActive=a.Status
                          }).FirstOrDefault();


            if (details != null)
            {
                return Ok(details );
            }
            return NotFound(new { success = false, message = "Account not found." });

        }

        [HttpGet("balance-check")]
        public IActionResult Balance_Inquary()
        {
            return PartialView("_BalanceInquary");
        }
        [HttpPost("balance-check")]
        public async Task<IActionResult> Balance_Inquary(long accountNumber, string secondary)
        {
            try
            {
                var accDetails =await (
                    from a in db.AccountMsts
                    join c in db.Customers on a.AccId equals c.AccId
                    join b in db.Branches on a.BranchId equals b.BranchId
                    join u in db.Users on a.AccId equals u.AccId into userGroup
                    from u in userGroup.DefaultIfEmpty()
                    where a.AccountNo == accountNumber
                    select new CheckBalanceDTO
                    {
                        AccId=a.AccId,
                        AccNo = a.AccountNo,
                        CustomerName = c.Name,
                        Mobile = Convert.ToInt64(u.PhoneNo),
                        AccType = a.AccType,
                        Balance = Convert.ToDecimal(a.Balance),
                        CreatedDt = a.CreatedDt.ToDateTime(TimeOnly.MinValue),
                        BranchName = b.BranchName,
                        IFSCCode = b.Ifsccode,
                        Status = a.Status == true ? "Active" : "Closed"
                    }
                ).FirstOrDefaultAsync();
                int currMonth=DateTime.Now.Month;
                int currYear=DateTime.Now.Year;
                var summary = await db.Transactions
                   .Where(t => t.FromAcId == accDetails.AccId || t.ToAcId==accDetails.AccId && t.Timestamp.Value.Month ==currMonth && t.Timestamp.Value.Year ==currYear)
                   .GroupBy(t => 1)
                   .Select(g => new
                   {
                       Deposit = g.Sum(t => t.Type == "Deposit" ? t.Amount : 0),
                       Withdraw = g.Sum(t => t.Type == "Withdrawal" ? t.Amount : 0), 
                       TransactionCount = g.Count()
                   })
                   .FirstOrDefaultAsync();

                var lastTransaction = await db.Transactions
                    .Where(t => t.FromAcId == accDetails.AccId||t.ToAcId==accDetails.AccId).Select(x => new LastTransactionDTO
                    {
                        FromAcId = x.FromAcId,
                        ToAcId = x.ToAcId,
                        Amount = x.Amount,
                        Type = x.Type,
                        Timestamp = x.Timestamp,
                        TransactionId = x.TransactionId

                    })
                    .OrderByDescending(t => t.Timestamp).Take(5)
                    .ToListAsync();

                return Json(new
                {
                    success = true,
                    accountNumber = accDetails?.AccNo,
                    holderName = accDetails?.CustomerName,
                    accountType = accDetails?.AccType,
                    balance = accDetails?.Balance ?? 0,
                    branchName = accDetails?.BranchName,
                    ifscCode = accDetails?.IFSCCode,
                    status = accDetails?.Status,
                    openingDate = accDetails?.CreatedDt.ToString("dd MMM yyyy"),
                    mobile = accDetails?.Mobile,
                    totalCredits = summary?.Withdraw ?? 0,
                    totalDebits = summary?.Deposit ?? 0,
                    totalTransactions = summary?.TransactionCount ?? 0,
                    lastTransactionDate = lastTransaction?.FirstOrDefault()?.Timestamp,
                    recentTransactions =lastTransaction
                    
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error : " + ex.Message, error = ex.Message });
            }
        }

        private List<SelectListItem> GetStates()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Andhra Pradesh", Value = "Andhra Pradesh" },
                new SelectListItem { Text = "Arunachal Pradesh", Value = "Arunachal Pradesh" },
                new SelectListItem { Text = "Assam", Value = "Assam" },
                new SelectListItem { Text = "Bihar", Value = "Bihar" },
                new SelectListItem { Text = "Chhattisgarh", Value = "Chhattisgarh" },
                new SelectListItem { Text = "Goa", Value = "Goa" },
                new SelectListItem { Text = "Gujarat", Value = "Gujarat" },
                new SelectListItem { Text = "Haryana", Value = "Haryana" },
                new SelectListItem { Text = "Himachal Pradesh", Value = "Himachal Pradesh" },
                new SelectListItem { Text = "Jharkhand", Value = "Jharkhand" },
                new SelectListItem { Text = "Karnataka", Value = "Karnataka" },
                new SelectListItem { Text = "Kerala", Value = "Kerala" },
                new SelectListItem { Text = "Madhya Pradesh", Value = "Madhya Pradesh" },
                new SelectListItem { Text = "Maharashtra", Value = "Maharashtra" },
                new SelectListItem { Text = "Manipur", Value = "Manipur" },
                new SelectListItem { Text = "Meghalaya", Value = "Meghalaya" },
                new SelectListItem { Text = "Mizoram", Value = "Mizoram" },
                new SelectListItem { Text = "Nagaland", Value = "Nagaland" },
                new SelectListItem { Text = "Odisha", Value = "Odisha" },
                new SelectListItem { Text = "Punjab", Value = "Punjab" },
                new SelectListItem { Text = "Rajasthan", Value = "Rajasthan" },
                new SelectListItem { Text = "Sikkim", Value = "Sikkim" },
                new SelectListItem { Text = "Tamil Nadu", Value = "Tamil Nadu" },
                new SelectListItem { Text = "Telangana", Value = "Telangana" },
                new SelectListItem { Text = "Tripura", Value = "Tripura" },
                new SelectListItem { Text = "Uttar Pradesh", Value = "Uttar Pradesh" },
                new SelectListItem { Text = "Uttarakhand", Value = "Uttarakhand" },
                new SelectListItem { Text = "West Bengal", Value = "West Bengal" },
            };
        }
        private List<SelectListItem> GetCountries()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "India", Value = "India" },
                new SelectListItem { Text = "United States", Value = "United States" },
                new SelectListItem { Text = "United Kingdom", Value = "United Kingdom" },
                new SelectListItem { Text = "Canada", Value = "Canada" },
                new SelectListItem { Text = "Australia", Value = "Australia" },
                new SelectListItem { Text = "Germany", Value = "Germany" },
                new SelectListItem { Text = "France", Value = "France" },
                new SelectListItem { Text = "Japan", Value = "Japan" },
                new SelectListItem { Text = "China", Value = "China" },
                new SelectListItem { Text = "Brazil", Value = "Brazil" }
            };
        }
        private List<SelectListItem> GetBranch()
        {
            return db.Branches.Select(b => new SelectListItem
            {
                Text = b.BranchName,
                Value = b.BranchCode.ToString()
            }).ToList();

        }
    }
}
