using Microsoft.AspNetCore.Mvc;

namespace Banking_Platform_2._0.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
