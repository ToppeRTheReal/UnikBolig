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
        public UserController(IUserHandler handler)
        {
            this.handler = handler;
        }
        public IActionResult Index()
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
            Guid UserID = Guid.Parse(HttpContext.Session.GetString("UserID"));
            this.handler.AuthenticateUser(UserID, HttpContext.Session.GetString("Token"));
            var details = this.handler.GetDetails(UserID);
            return View(details);
        }
    }
}
