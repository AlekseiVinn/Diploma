using Microsoft.AspNetCore.Mvc;
using ScopeLap.DataBaseEngine;
using ScopeLap.Models;
using System.Diagnostics;

namespace ScopeLap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ScopeLapDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ScopeLapDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
