using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BnsBazarApp.Models;
using BnsBazarApp.Models.Data;

namespace BnsBazarApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BnsBazarDbContext _db;

        public HomeController(ILogger<HomeController> logger, BnsBazarDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        // PUBLIC HOME PAGE – no login required
        public IActionResult Index()
        {
            var allAds = _db.Advertisements
                            .OrderByDescending(a => a.PostedDate)
                            .ToList();

            return View(allAds);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // OPTIONAL: Settings page still requires login
        public IActionResult Settings()
        {
            string authenticated = HttpContext.Session.GetString("Authenticated") ?? "false";

            if (authenticated != "true")
            {
                return RedirectToAction("Login", "Authentication");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
