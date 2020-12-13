using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Application;
using UnikBolig.Api.Requests;
using UnikBolig.Api.Response;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("estates")]
    public class EstateController : ControllerBase
    {

        private readonly IEstateHandler handler;

        public EstateController(IEstateHandler handler)
        {
            this.handler = handler;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] API.Requests.CreateEstateRequest request)
        {
            try
            {
                this.handler.Create(request.Estate, request.Token);
                return Ok();
            }catch (Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }

        [HttpPost]
        [Route("Update")]
        public IActionResult Update([FromBody] UpdateEstateRequest request)
        {
            try
            {
                this.handler.Update(request.EstateID, request.Estate, request.UserID, request.Token);
                return Ok();
            } catch (Exception e)
            {
                ErrorResponse error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);
            }
        }

        [HttpGet]
        [Route("get/{ID}")]
        public IActionResult GetEstate(Guid ID)
        {
            var estate = this.handler.GetByID(ID);
            if (estate == null)
                return NotFound();
            else
                return Ok(estate);
        }
    }
}