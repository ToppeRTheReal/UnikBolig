using System;
using Microsoft.AspNetCore.Mvc;
using UnikBolig.Application;
using UnikBolig.Models;
using UnikBolig.Models.Requests;
using UnikBolig.Api.Response;

namespace UnikBolig.Api.Controllers
{
    [ApiController]
    [Route("Rulesets")]
    public class RulesetController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] Models.Requests.RulesetRequest request)
        {
            try
            {
                var handler = new RulesetHandler();
                handler.Create(request.Ruleset, request.Token);
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
