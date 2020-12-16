using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unik.Models;
using UnikBolig.Models;
using UnikBolig.Application;
namespace Unik.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> _logger;
        IUserHandler handler;
        IEstateHandler estateHandler;


        public HomeController(ILogger<HomeController> logger, IUserHandler handler, IEstateHandler eHandler)
        {
            _logger = logger;
            this.handler = handler;
            this.estateHandler = eHandler;
        }

        [Route("/")]
        public IActionResult Index()
        {
            var Estates = this.estateHandler.GetAll();
            return View("/Views/Home/Index.cshtml", Estates);
        }

        [Route("bolig/{ID}")]
        public IActionResult Estate([FromRoute] Guid ID)
        {
            var response = this.estateHandler.GetAll();
            return View(response);
        }

        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Udlej()
        {
            return View();
        }

        public IActionResult Estate()
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
