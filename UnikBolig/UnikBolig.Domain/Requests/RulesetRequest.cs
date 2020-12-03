using System;

namespace UnikBolig.Models.Requests
{
    public class RulesetRequest
    {
        public EstateRulesetModel Ruleset { get; set; }
        public string Token { get; set; }
    }
}
