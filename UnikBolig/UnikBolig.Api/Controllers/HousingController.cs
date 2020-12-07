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

        IHousingHandler handler = new HousingHandler(null);

        [HttpPost]
        [Route("add")]
        public IActionResult AddToWaitingList([FromBody] WaitingList list, [FromBody] string Token)
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
    }
}
