using Banking_Platform_2._0.Models;
using Banking_Platform_2._0.Models.DTO_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace Banking_Platform_2._0.Controllers
{
    public class AdminController : Controller
    {
        private readonly BankingDbContext db;
        public AdminController(BankingDbContext _db)
        {
            db = _db;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Deposit()
        {
            Transaction tr = new Transaction();
            return PartialView("_Deposit", tr);
        }
        [HttpPost]
        public IActionResult Deposit(Transaction tr)
        {
            return PartialView("_Deposit");
        }
        public IActionResult Withdraw()
        {
            Transaction tr = new Transaction();
            return PartialView("_Withdraw", tr);
        }
        [HttpPost]
        public IActionResult Withdraw(Transaction tr)
        {
            return PartialView("_Withdraw", tr);
        }
        public IActionResult Transfer()
        {
            Transaction tr = new Transaction();
            return PartialView("_Transfer", tr);
        }
        [HttpPost]
        public IActionResult Transfer(Transaction tr)
        {
            return PartialView("_Transfer", tr);
        }
        public IActionResult New_Account()
        {
            ViewBag.States = GetStates();
            ViewBag.Countries = GetCountries();
            ViewBag.Branch = GetBranch();
            NewAccountDTO tr = new NewAccountDTO();
            return PartialView("_NewAccount", tr);
        }
        [HttpPost]
        public IActionResult New_Account(NewAccountDTO ac)
        {
            if (InsertIntoUser(ac) > 0)
            {
                int UId = db.Users.Where(u => u.Email == ac.Email && u.PhoneNo == ac.MobileNumber.ToString()).FirstOrDefault().UserId;
                if (InsertIntoAcc(ac, UId) > 0)
                {
                    InsertIntoCustomer(ac, UId);
                }
            }


            ViewBag.States = GetStates();
            ViewBag.Countries = GetCountries();
            ViewBag.Branch = GetBranch();
            return PartialView("_NewAccount", ac);
        }
        private int InsertIntoAcc(NewAccountDTO ac, int uid)
        {
            AccountMst account = new AccountMst()
            {
                AccType = ac.AccountType,
                Balance = Convert.ToDecimal(ac.InitialDeposit),
                BranchId = ac.BranchCode,
                Status = true,
                AccountNo = CreatAccNo(),
                //CreatedDt = DateTime.Now,
            };
            db.AccountMsts.Add(account);
            return db.SaveChanges();
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
        [HttpGet]
        public JsonResult GetIfscByBranchId(string branchId)
        {
            // Example: lookup from DB
            var ifscCode = db.Branches
                .Where(b => b.BranchCode == branchId)
                .Select(b => b.Ifsccode)
                .FirstOrDefault();

            return Json(new { ifsc = ifscCode, branchId });
        }

        public IActionResult Modify()
        {
            Transaction tr = new Transaction();
            return PartialView("_Modify_Account", tr);
        }
        [HttpPost]
        public IActionResult Modify(Transaction tr)
        {
            return PartialView("_Modify_Account", tr);
        }
        public IActionResult Balance_Inquary()
        {
            Transaction tr = new Transaction();
            return PartialView("_Balance_Inquary", tr);
        }
        [HttpPost]
        public IActionResult Balance_Inquary(Transaction tr)
        {
            return PartialView("_Balance_Inquary", tr);
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
