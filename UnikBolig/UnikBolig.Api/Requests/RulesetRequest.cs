using System;
using UnikBolig.Models;

namespace UnikBolig.API.Requests
{
    public class RulesetRequest
    {
        public EstateRulesetModel Ruleset { get; set; }
        public string Token { get; set; }
    }
}
