using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnikBolig.Application;
using Microsoft.AspNetCore.Http;
using UnikBolig.Web.Models;

namespace UnikBolig.Web.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        IUserHandler handler;
        IHousingHandler HousingHandler;
        public UserController(IUserHandler handler, IHousingHandler _handler)
        {
            this.handler = handler;
            this.HousingHandler = _handler;
        }
        public IActionResult Index()
        {
            return View();
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
        public IActionResult Estate([FromRoute] Guid EstateID)
        {
            return View();
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
                HttpContext.Session.SetString("UserID", token.UserID.ToString());
                HttpContext.Session.SetString("Token", token.Token);
                return View("/Views/Home/Index.cshtml");
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
    }
}
