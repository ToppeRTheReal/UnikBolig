using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnikBolig.Api.Response;
using UnikBolig.Application;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("landlord")]
    public class LandlordController : Controller
    {
        private readonly IHousingHandler handler;

        public LandlordController(IHousingHandler _handler)
        {
            this.handler = _handler;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("getqualifiers")]
        public IActionResult GetQualifiers([FromBody] GetQualifiersModel Model)
        {
            try
            {
                var x = this.handler.GetHousingQualifiers(Model.EstateID, Model.UserID, Model.Token);
                return Ok(x);
            }catch(Exception e)
            {
                var err = new ErrorResponse();
                err.Message = e.Message;
                return StatusCode(403, err);
            }
        }

        [HttpPost]
        [Route("movein")]
        public IActionResult MoveIn([FromBody] MoveInRequest Request)
        {
            try
            {
                this.handler.MoveIn(Request.OwnerID, Request.Token, Request.UserID, Request.EstateID);
                return Ok();
            }catch(Exception e)
            {
                var err = new ErrorResponse();
                err.Message = e.Message;
                return NotFound(e);
            }
        }

        [HttpPost]
        [Route("moveout")]
        public IActionResult MoveOut([FromBody] MoveInRequest request)
        {
            try
            {
                this.handler.MoveOut(request.OwnerID, request.Token, request.EstateID);
                return Ok();
            }catch (Exception e)
            {
                var err = new ErrorResponse();
                err.Message = e.Message;
                return Unauthorized(err);
            }
        }
    }

    public class GetQualifiersModel
    {
        public Guid EstateID { get; set; }
        public Guid UserID { get; set; }
        public string Token { get; set; }
    }

    public class MoveInRequest
    {
        public Guid EstateID { get; set; }
        public Guid OwnerID { get; set; }
        public Guid UserID { get; set; }
        public string Token { get; set; }
    }

    public class MoveOutRequest
    {
        public Guid OwnerID { get; set; }
        public string Token { get; set; }
        public Guid EstateID { get; set; }
    }
}
