using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using Banking_Platform_2._0.Models;
using Banking_Platform_2._0.Models.DTO_Models;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
namespace Banking_Platform_2._0.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly BankingDbContext db;
        public UserController(BankingDbContext _db)
        {
            db = _db;
        }
        [HttpGet("user-register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("verify-account")]
        public IActionResult VerifyAccount(RegistrationDTO reg)
        {
            /*try
            {
                var branch = db.Branches.Where(b => b.Ifsccode == reg.IFSC).FirstOrDefault();

                if (branch != null)
                {
                    var branch_id = branch.BranchId;

                    if (branch_id > 0)
                    {
                        var account = db.AccountMsts
                            .Where(a => a.AccountNo == reg.AccountNumber && a.BranchId == branch_id)
                            .FirstOrDefault();

                        if (account != null)
                        {
                            var acc_id = account.AccId;

                            if (acc_id > 0)
                            {
                                var user = db.Customers
                                    .Where(u => u.AccId == acc_id)
                                    .FirstOrDefault();

                                if (user != null)
                                {
                                    return Json(new
                                    {
                                        success = true,
                                        fullName = user.Name
                                    });
                                }
                                else
                                {
                                    return Json(new
                                    {
                                        success = false,
                                        message = "Customer record not found for this account."
                                    });
                                }
                            }
                        }
                        else
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Account not found in this branch."
                            });
                        }
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid IFSC code."
                    });
                }

                return Json(new
                {
                    success = false,
                    message = "Invalid Account Number or IFSC."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while verifying the account."
                });
            }*/
            try
            {
                var user = (from b in db.Branches
                            join a in db.AccountMsts
                            on b.BranchId equals a.BranchId
                            join c in db.Customers
                            on a.AccId equals c.AccId
                            where b.Ifsccode == reg.IFSC && a.AccountNo == reg.AccountNumber
                            select new
                            {
                                c.Name,
                                a.AccId
                            }).FirstOrDefault();
                if (user == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid Account Number / IFSC Code"
                    });
                }

                var userDetails = db.Users.FirstOrDefault(u => u.AccId == user.AccId);

                if (userDetails != null && userDetails.Status)
                {
                    return Json(new
                    {
                        success = false,
                        message = "User Already Registered!"
                    });
                }

                return Json(new
                {
                    success = true,
                    fullName = user.Name
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpPost("user-register")]
        public IActionResult Register(RegistrationDTO reg)
        {
            try
            {
                if (reg.Password != reg.ConfirmPassword)
                {
                    return Json(new { success = false, message = "Passwords do not match" });
                }

                User user = new User();
                user.Username = reg.UserName;
                user.Password = reg.Password;
                user.Email = reg.Email;
                user.PhoneNo = reg.Phone;
                user.Status = true;
                user.RoleId = db.Roles.Where(r => r.RoleName == "Customer").FirstOrDefault().RoleId;

                db.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login", "Login");
            }
            catch (Exception err)
            {
                return Json(new { message = err.Message });
            }
        }
        [HttpGet("home")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
