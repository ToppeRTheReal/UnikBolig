using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Models;
using UnikBolig.Application;
using UnikBolig.Api.Response;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        IUserHandler handler = new UserHandler(null);

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] UserModel User)
        {
            try
            {
                this.handler.Create(Guid.NewGuid(), User.FirstName, User.LastName, User.Email, User.Phone, User.Password);
                UserResponse response = new UserResponse();
                response.Message = "Success, please login now";
                return Ok(response);
            }catch(Exception e)
            {
                ErrorResponse error = new ErrorResponse();
                error.Message = e.Message;
                return Conflict(error);
            }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserModel User)
        {
            try
            {
                TokenModel UserToken = this.handler.Login(User.Email, User.Password);
                var Response = new UserResponse();
                Response.Message = "Success";
                Response.Token = UserToken.Token;
                Response.UserID = UserToken.UserID;

                return Ok(Response);
            }
            catch (Exception e)
            {
                var Error = new ErrorResponse();
                Error.Message = e.Message;
                return Unauthorized(Error);
            }
        }

        [HttpPost]
        [Route("updateusertype")]
        public IActionResult UpdateUserType([FromBody] API.Requests.UpdateUserTypeRequest Request)
        {
            try
            {
                this.handler.ChangeUserType(Request.UserID, Request.UserToken, Request.NewType);
                return Ok();
            }catch (Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }

        [HttpPost]
        [Route("details")]
        public IActionResult CreateUpdateUserDetails([FromBody] API.Requests.UserDetailsRequest Request)
        {
            try
            {
                this.handler.CreateUpdateUserDetails(Request.Details, Request.Token);
                return Ok();
            }catch(Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }
    }
}
