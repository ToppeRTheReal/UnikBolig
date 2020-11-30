﻿using System;
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
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] UserModel User)
        {
            try
            {
                UserHandler handler = new UserHandler();
                handler.CreateUser(Guid.NewGuid(), User.FirstName, User.LastName, User.Email, User.Phone, User.Password);
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
        [Route("Login")]
        public IActionResult Login([FromBody] UserModel User)
        {
            try
            {
                UserHandler handler = new UserHandler();
                TokenModel UserToken = handler.Login(User.Email, User.Password);
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
        [Route("UpdateUserType")]
        public IActionResult UpdateUserType([FromBody] Models.Requests.UpdateUserTypeRequest Request)
        {
            UserHandler handler = new UserHandler();
            try
            {
                handler.ChangeUserType(Request.UserID, Request.UserToken, Request.NewType);
                return Ok();
            }catch (Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }

        [HttpPost]
        [Route("Details")]
        public IActionResult CreateUpdateUserDetails([FromBody] Models.Requests.UserDetailsRequest Request)
        {
            var handler = new UserHandler();
            try
            {
                handler.CreateUpdateUserDetails(Request.Details, Request.Token);
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
