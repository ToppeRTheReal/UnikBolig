using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Application;
using UnikBolig.Models;
using UnikBolig.Api.Response;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("Estates")]
    public class EstateController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Models.Requests.CreateEstateRequest request)
        {
            try
            {
                var handler = new EstateHandler();
                handler.Create(request.Estate, request.Token);
                return Ok();
            }catch (Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }

        [HttpGet]
        [Route("get/{ID}")]
        public IActionResult GetEstate([FromRoute] Guid ID)
        {
            var Handler = new EstateHandler();
            return Ok(Handler.GetHouseByID(ID));
        }
    }
}
