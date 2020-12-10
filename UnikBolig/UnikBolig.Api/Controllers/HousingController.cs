using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Application;
using UnikBolig.Models;
using UnikBolig.Api.Response;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("waitinglist")]
    public class HousingController : ControllerBase
    {
        IHousingHandler handler;

        public HousingController(IHousingHandler handler)
        {
            this.handler = handler;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddToWaitingList([FromBody] WaitingList list, string Token)
        {
            try
            {
                this.handler.Create(list, Token);
                return Ok();
            }catch(Exception e)
            {
                var err = new ErrorResponse();
                err.Message = e.Message;
                return Unauthorized(err);
            }
        }

        [HttpPost]
        [Route("remove")]
        public  IActionResult RemoveFromWaitingList([FromBody] Guid UserID, string Token, Guid EstateID)
        {
            try
            {
                this.handler.Remove(UserID, Token, EstateID);
                return Ok();
            }catch (Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }

        [HttpPost]
        [Route("gethousings")]
        public IActionResult GetHousesWrittinUpFor([FromBody] Guid UserID, string Token)
        {
            try
            {
                var houses = this.handler.GetAllHousingsWrittenUpFor(UserID, Token);
                return Ok(houses);
            }catch (Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }
    }
}
