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
    public class LandlordController : Controller
    {
        IEstateHandler estateHandler;
        IUserHandler userHandler;
        IRulesetHandler rulesetHandler;

        public LandlordController(IEstateHandler _estateHandler, IUserHandler uhandler, IRulesetHandler rhandler)
        {
           
            this.estateHandler = _estateHandler;
            this.userHandler = uhandler;
            this.rulesetHandler = rhandler;
        }

        [Route("/landlord")]
        public IActionResult Index()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");

                var user = this.userHandler.GetByID(UserID, Token);

                if (user.Type != "landlord")
                    throw new Exception("Der skete en fejl, prøv at logge ind igen");

                ViewBag.Rulesets = this.rulesetHandler.GetOwnedRuleset(UserID, Token);
                ViewBag.Estates = this.estateHandler.GetOwnedEstates(UserID, Token);

                return View();
            }catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/Home/Login.cshtml");
            }
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
            catch(Exception)
            {
                ViewBag.Message = "Der er sket en fejl";
                return View();
            }
           
        }

        [Route("estates/create")]
        public IActionResult Estate()
        {
            return View();
        }

        [Route("rulesets/create")]
        public IActionResult RulesetCreateView()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");

                var user = this.userHandler.GetByID(UserID, Token);

                if (user.Type != "landlord")
                    throw new Exception("Der skete en fejl, prøv at logge ind igen");

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/Home/Login.cshtml");
            }
        }

        [HttpPost]
        [Route("rulesets/createruleset")]
        public IActionResult CreateRuleset(EstateRulesetModel Model)
        {
            try
            {
                Model.UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                this.rulesetHandler.Create(Model, HttpContext.Session.GetString("Token"));
                ViewBag.Message = "Regelsæt oprettet";
            }catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }

            return View("/Views/Landlord/Index.cshtml");
        }

        [Route("rulesets")]
        public IActionResult Rulesets()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");

                var user = this.userHandler.GetByID(UserID, Token);

                if (user.Type != "landlord")
                    throw new Exception("Der skete en fejl, prøv at logge ind igen");

                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/Home/Login.cshtml");
            }
        }
    }
}