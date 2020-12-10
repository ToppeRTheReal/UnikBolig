using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Application;
using UnikBolig.Models;
using UnikBolig.Api.Response;
using UnikBolig.Api.Requests;

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
        public IActionResult AddToWaitingList([FromBody] Requests.AddToWaitingListRequest request)
        {
            try
            {
                this.handler.Create(request.list, request.Token);
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
        public  IActionResult RemoveFromWaitingList([FromBody] Requests.RemoveFromWaitingListRequest request)
        {
            try
            {
                this.handler.Remove(request.UserID, request.Token, request.EstateID);
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
        public IActionResult GetHousesWrittinUpFor([FromBody] Requests.GetHousesWrittenUpForRequest request)
        {
            try
            {
                var houses = this.handler.GetAllHousingsWrittenUpFor(request.UserID, request.Token);
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
