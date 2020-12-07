using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Application;
using UnikBolig.Models;
using UnikBolig.Api.Response;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("Rulesets")]
    public class RulesetController : ControllerBase
    {

        IRulesetHandler handler = new RulesetHandler(null);

        [HttpPost]
        [Route("Create")]
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
