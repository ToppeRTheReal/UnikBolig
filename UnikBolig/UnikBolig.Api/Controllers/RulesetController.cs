using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Application;
using UnikBolig.Models;
using UnikBolig.Api.Response;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("rulesets")]
    public class RulesetController : ControllerBase
    {
        private readonly IRulesetHandler handler;

        public RulesetController(IRulesetHandler handler)
        {
            this.handler = handler;
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] API.Requests.RulesetRequest request)
        {
            try
            {
                this.handler.Create(request.Ruleset, request.Token);
                return Ok();
            }catch (Exception e)
            {
                var error = new ErrorResponse();
                error.Message = e.Message;
                return Unauthorized(error);

            }
        }
    }
}
