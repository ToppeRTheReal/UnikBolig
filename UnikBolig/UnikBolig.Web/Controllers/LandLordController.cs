using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnikBolig.Application;
using UnikBolig.Models;

namespace UnikBolig.Web.Controllers
{
    [Route("landlord")]
    public class LandLordController : Controller
    {
        IEstateHandler estateHandler;

        public LandLordController(IEstateHandler _estateHandler)
        {
           
            this.estateHandler = _estateHandler;
        }


        [HttpPost]
        [Route("createEstate")]
        public IActionResult CreateEstate(EstateModel Model)
        {
            try
            {
                Model.UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");
                this.estateHandler.Create(Model, Token);
                ViewBag.Message = "Bolig Oprette";
                return View();
            }
            catch(Exception e)
            {
                ViewBag.Message = "Der er sket en fejl";
                return View();
            }
           
        }
        [Route("estate")]
       public IActionResult Estate()
        {
            return View();
        }
    }
}
