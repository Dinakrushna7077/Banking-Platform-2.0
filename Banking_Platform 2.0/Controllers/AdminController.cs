using Banking_Platform_2._0.Models;
using Banking_Platform_2._0.Models.DTO_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

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
        [HttpGet("deposite")]
        public IActionResult Deposit()
        {
            DepositeDTO tr = new DepositeDTO();
            return PartialView("_Deposite", tr);
        }
        [HttpPost("deposite")]
        public IActionResult Deposit(Transaction tr)
        {
            return PartialView("_Deposit");
        }
        [HttpGet("withdraw")]
        public IActionResult Withdraw()
        {
            WithdrwalDTO tr = new WithdrwalDTO();
            return PartialView("_Withdraw", tr);
        }
        [HttpPost("withdraw")]
        public IActionResult Withdraw(Transaction tr)
        {
            return PartialView("_Withdraw", tr);
        }
        [HttpGet("transfer")]
        public IActionResult Transfer()
        {
            TransactionDTO tr = new TransactionDTO();
            return PartialView("_Transaction", tr);
        }
        [HttpPost("transfer")]
        public IActionResult Transfer(Transaction tr)
        {
            return PartialView("_Transaction", tr);
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
            // Example: lookup from DB
            var ifscCode = db.Branches
                .Where(b => b.BranchCode == branchId)
                .Select(b => b.Ifsccode)
                .FirstOrDefault();

            return Json(new { ifsc = ifscCode, branchId });
        }
        [HttpGet("modify-account")]
        public IActionResult Modify()
        {
            ModifyAccountDto ac = new ModifyAccountDto();
            return PartialView("_Modify", ac);
        }
        [HttpPost]
        public IActionResult Modify(ModifyAccountDto ac)
        {
            return PartialView("_Modify_Account", ac);
        }
        [HttpGet("balance-check")]
        public IActionResult Balance_Inquary()
        {
            return PartialView("_BalanceInquary");
        }
        [HttpPost("balance-check")]
        public IActionResult Balance_Inquary(long accountNumber,string secondary)
        {
            //return PartialView("_Balance_Inquary", tr);
            var accDetails = (
                from a in db.AccountMsts
                join c in db.Customers on a.AccId equals c.AccId
                join b in db.Branches on a.BranchId equals b.BranchId
                join u in db.Users on a.AccId equals u.AccId into userGroup
                from u in userGroup.DefaultIfEmpty()
                where a.AccountNo == accountNumber
                select new CheckBalanceDTO
                {
                    AccNo = a.AccountNo,
                    CustomerName = c.Name,
                    Mobile = Convert.ToInt64(u.PhoneNo),
                    AccType = a.AccType,
                    Balance = Convert.ToDecimal(a.Balance),
                    CreatedDt = Convert.ToDateTime(a.CreatedDt),
                    BranchName = b.BranchName,
                    IFSCCode = b.Ifsccode,
                    Status = a.Status == true ? "Active" : "Closed"
                }
            ).ToListAsync();
            //-------------
            var summary = db.Transactions
               .Where(t => t.FromAcId == accountNumber)
               .GroupBy(t => 1)
               .Select(g => new
               {
                   Deposit = g.Sum(t => t.Type == "Deposit" ? t.Amount : 0),
                   Withdraw = g.Sum(t => t.Type == "Withdrawal" ? t.Amount : 0), // ✅ fix
                   TransactionCount = g.Count()
               })
               .FirstOrDefaultAsync();

            var lastTransaction = db.Transactions
                .Where(t => t.FromAcId == accountNumber).Select(x=>new LastTransactionDTO
                {
                    FromAcId = x.FromAcId,
                    ToAcId = x.ToAcId,
                    Amount = x.Amount,
                    Type = x.Type,
                    Timestamp = x.Timestamp

                })
                .OrderByDescending(t => t.Timestamp)
                .FirstOrDefault();

            return Json(new
                {
                    success = true,
                    accountNumber = "123456789012",//done
                    holderName = "John Doe",//done
                    accountType = "Saving",//done
                    balance = 25000.00,//done
                    branchName = "Main Branch",//done
                    ifscCode = "BANK0001234",//done
                    status = "Active",//done
                    openingDate = "01 Jan 2022",//done
                    mobile = "98XXXXXX01",
                    totalCredits = summary.Result?.Withdraw ?? 0,
                    totalDebits = summary.Result?.Deposit ?? 0,//done
                    totalTransactions = 42,//done
                    lastTransactionDate = "10 Mar 2026",//done
                    recentTransactions = new[] {
                    new { date = "10 Mar", description = "UPI Transfer", type = "Credit", amount = 5000 },
                    // ...
                    }
                });
           
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
