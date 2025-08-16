using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookWorld.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            var userRole = User?.Identity?.IsAuthenticated == true ? User.IsInRole("Admin") ? "Admin" : "User" : null;
            ViewData["UserRole"] = userRole;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "User")]
        public IActionResult UserDashboard()
        {
            return RedirectToAction("Dashboard");
        }
    }
}