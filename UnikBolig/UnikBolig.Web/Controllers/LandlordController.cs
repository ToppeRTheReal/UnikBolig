using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnikBolig.Application;
using UnikBolig.Models;

namespace UnikBolig.Web.Controllers
{
    [Route("landlord")]
    public class LandlordController : Controller
    {
        IEstateHandler estateHandler;
        IHousingHandler handler;
        IUserHandler userHandler;
        IRulesetHandler rulesetHandler;

        public LandlordController(IEstateHandler _estateHandler, IUserHandler uhandler, IRulesetHandler rhandler, IHousingHandler lHandler)
        {
           
            this.estateHandler = _estateHandler;
            this.userHandler = uhandler;
            this.rulesetHandler = rhandler;
            this.handler = lHandler;
        }

        [Route("/landlord")]
        public IActionResult Index()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");

                var user = this.userHandler.GetByID(UserID);

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
                ViewBag.Rulesets = this.rulesetHandler.GetOwnedRuleset(Model.UserID, Token);
                ViewBag.Estates = this.estateHandler.GetOwnedEstates(Model.UserID, Token);
                ViewBag.Message = "Bolig Oprette";
                return View("/Views/Landlord/Index.cshtml");
            }
            catch(Exception)
            {
                ViewBag.Message = "Der er sket en fejl";
                return View("/Views/Landlord/Estate.cshtml");
            }
        }

        [Route("estates/create")]
        public IActionResult Estate()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");

                ViewBag.Rulesets = this.rulesetHandler.GetOwnedRuleset(UserID, Token);
                return View();
            }catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/Home/Login.cshtml");
            }
        }

        [Route("rulesets/create")]
        public IActionResult RulesetCreateView()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");

                var user = this.userHandler.GetByID(UserID);

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

            Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
            string Token = HttpContext.Session.GetString("Token");

            ViewBag.Rulesets = this.rulesetHandler.GetOwnedRuleset(UserID, Token);
            ViewBag.Estates = this.estateHandler.GetOwnedEstates(UserID, Token);
            return View("/Views/Landlord/Index.cshtml");
        }

        [Route("rulesets")]
        public IActionResult Rulesets()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");

                var user = this.userHandler.GetByID(UserID);

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

        [Route("qualifiers/{ID}")]
        public IActionResult GetQualifiers([FromRoute] Guid ID) {
            try
            {
                var estate = this.estateHandler.GetByID(ID);
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string token = HttpContext.Session.GetString("Token");

                if(!estate.IsRented){
                    ViewBag.Qualifiers = this.handler.GetHousingQualifiers(ID, UserID, token);
                }else {
                    var usr = this.userHandler.GetByID((Guid) estate.CurrentRenter);
                    ViewBag.User = usr;
                    ViewBag.EstateID = estate.ID;
                }
                
            }catch(Exception e)
            {
                ViewBag.Message = e.Message;
            }

            return View("/Views/Landlord/Qualifiers.cshtml");
        }

        [HttpGet]
        [Route("movein/{estateID}/{UserID}")]
        public IActionResult MoveIn([FromRoute] Guid estateID, Guid UserID)
        {
            try
            {
                Guid Owner = Guid.Parse(HttpContext.Session.GetString("UserID"));
                this.handler.MoveIn(Owner, HttpContext.Session.GetString("Token"), UserID, estateID);
                ViewBag.Message = "Brugeren er nu flyttet ind";
            }catch(Exception e)
            {
                ViewBag.Message = e.Message;
            }

            return View("/Views/User/Index.cshtml");
        }

        [HttpGet]
        [Route("moveout/{estateID}")]
        public IActionResult MoveOut([FromRoute] Guid estateID)
        {
            try
            {
                Guid Owner = Guid.Parse(HttpContext.Session.GetString("UserID"));
                this.handler.MoveOut(Owner, HttpContext.Session.GetString("Token"), estateID);
                ViewBag.Message = "Brugeren er nu flyttet ud, og enheden står nu til leje igen";
            }catch (Exception e)
            {
                ViewBag.Message = e.Message;
            }

            return View("/Views/User/Index.cshtml");
        }
    }
    public class MoveInRequest
    {
        public Guid EstateID { get; set; }
        public Guid OwnerID { get; set; }
        public Guid UserID { get; set; }
        public string Token { get; set; }
    }
}