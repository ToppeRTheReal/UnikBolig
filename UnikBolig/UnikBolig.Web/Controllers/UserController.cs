using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnikBolig.Application;
using Microsoft.AspNetCore.Http;
using UnikBolig.Web.Models;
using UnikBolig.Models;

namespace UnikBolig.Web.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        IUserHandler handler;
        IHousingHandler HousingHandler;
        IEstateHandler estateHandler;
        public UserController(IUserHandler handler, IHousingHandler _handler, IEstateHandler _estateHandler)
        {
            this.handler = handler;
            this.HousingHandler = _handler;
            this.estateHandler = _estateHandler;
        }
        public IActionResult Index()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");
                if (!this.handler.AuthenticateUser(UserID, Token))
                    throw new Exception();
                return View();
            }catch
            {
                ViewBag.Message = "Der skete en fejl, prøv at logge ind igen";
                return View("/Views/Home/Login.cshtml");
            }
        }

        [Route("writeups")]
        public IActionResult WrittenUpEstates()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");
                var response = this.HousingHandler.GetAllHousingsWrittenUpFor(UserID, Token);
                return View(response);
            }catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/Home/Login.cshtml");
            }
        }

        [Route("bolig/{ID}")]
        public IActionResult Estate([FromRoute] Guid ID)
        {
            var response = this.estateHandler.GetAll();
            return View(response);
        }

        [HttpPost]
        [Route("BecomeLandlord")]
        public IActionResult BecomeLandlord()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                string Token = HttpContext.Session.GetString("Token");
                this.handler.ChangeUserType(UserID, Token, "landlord");
                HttpContext.Session.SetString("Type", "landlord");
                ViewBag.Message = "Du er nu landlord";
                return View("/Views/User/Index.cshtml");
            }catch(ArgumentNullException)
            {
                ViewBag.Message = "Det skete en ukendt fejl prøv at logge ind igen";
                return View("/Views/Home/Login.cshtml");
            }catch(FormatException)
            {
                ViewBag.Message = "Det skete en ukendt fejl prøv at logge ind igen";
                return this.Index();
            }catch
            {
                ViewBag.Message = "Der skete en ukendt fejl prøv igen";
                return this.Index();
            }
        }

        [HttpPost]
        [Route("registerUser")]
        public IActionResult RegisterUser(RegisterModel Model)
        {
            try
            {
                this.handler.Create(Guid.NewGuid(), Model.FirstName, Model.LastName, Model.Email, Model.Phone, Model.Password);
                return View("/Views/Home/Login.cshtml");
            }catch(Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/Home/Register.cshtml");
            }
            
        }

        [HttpPost]
        [Route("loginUser")]
        public IActionResult LoginUser(LoginModel Model)
        {
            try
            {
                var token = this.handler.Login(Model.Email, Model.Password);
                var usr = this.handler.GetByID(token.UserID, token.Token);
                HttpContext.Session.SetString("UserID", token.UserID.ToString());
                HttpContext.Session.SetString("Token", token.Token);
                HttpContext.Session.SetString("FirstName", usr.FirstName);
                HttpContext.Session.SetString("LastName", usr.LastName);
                HttpContext.Session.SetString("Type", usr.Type);
                return RedirectToAction("Index");
            }catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/Home/Login.cshtml");
            }
        }

        [Route("details")]
        public IActionResult Details()
        {
            try
            {
                Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
                var details = this.handler.GetDetails(UserID, HttpContext.Session.GetString("Token"));
                return View(details);
            }catch
            {
                return View("/Views/Home/Login.cshmtl");
            }
            
        }

        [HttpPost]
        [Route("SaveUserDetails")]
        public IActionResult UserDetailsSave(UserDetailModel Model)
        {
            try
            {
                this.handler.CreateUpdateUserDetails(Model, HttpContext.Session.GetString("Token"));
                ViewBag.Message("Dine detaljer blev gemt" );
                return View("/Views/User/Index.cshtml");
            }catch(Exception e)
            {
                ViewBag.Message = e.Message;
                return View("/Views/User/Index.cshtml");
            }
        }

     
    }
}
