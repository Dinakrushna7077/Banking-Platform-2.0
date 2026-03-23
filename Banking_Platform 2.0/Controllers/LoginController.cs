using Banking_Platform_2._0.Models;
using Banking_Platform_2._0.Models.DTO_Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Banking_Platform_2._0.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private readonly BankingDbContext db;
        public LoginController(BankingDbContext _db)
        {
            db = _db;
        }
        [HttpGet("user-login")]
        public IActionResult Login()
        {
            LoginDTO login = new LoginDTO();
            return View(login);
        }
        [HttpPost("user-login")]
        public IActionResult Login(LoginDTO u)
        {
            if (IsValidEmail(u.Identifier))
            {
                var user = db.Users.Where(x => x.Email == u.Identifier).FirstOrDefault();
                if (user != null)
                {
                    if (user.Password == u.Password)
                    {
                        HttpContext.Session.SetInt32("UserID", user.UserId);
                        HttpContext.Session.SetString("Username", user.Username);
                        HttpContext.Session.SetInt32("Role",user.RoleId);
                        return user.RoleId switch
                        {
                            1 => RedirectToAction("home", "Admin"),
                            2 => RedirectToAction("home", "Admin"),
                            3 => RedirectToAction("home", "User")
                        };
                    }
                    else
                    {
                        ViewBag.PassError = "Invalid Password";
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid Mail ID";
                }
            }
            else
            {
                var user = db.Users.Where(x => x.Username == u.Identifier).FirstOrDefault();
                if (user != null)
                {
                    if (user.Password == u.Password)
                    {
                        HttpContext.Session.SetInt32("UserID", user.UserId);
                        HttpContext.Session.SetString("Username", user.Username);
                        HttpContext.Session.SetInt32("Role", user.RoleId);
                        return user.RoleId switch
                        {
                            1 => RedirectToAction("home", "Admin"),
                            2 => RedirectToAction("home", "Admin"),
                            3 => RedirectToAction("home", "User")
                        };
                    }
                    else
                    {
                        ViewBag.PassError = "Invalid Password";
                    }
                }
                else
                {
                    ViewBag.Error = "Invalid User Name";
                }
            }
            return View(u);
        }
        private bool IsValidEmail(string input)
        {
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Session");
            return RedirectToAction("Index", "Home");
        }
    }
}
