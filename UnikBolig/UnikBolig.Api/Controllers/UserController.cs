using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Models;
using UnikBolig.Application;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserModel User)
        {
            try
            {
                UserHandler handler = new UserHandler();
                handler.CreateUser(Guid.NewGuid(), User.FirstName, User.LastName, User.Email, User.Phone, User.Password);
                return Ok();
            }catch(Exception e)
            {
                return Conflict(e);
            }
        }
    }
}
