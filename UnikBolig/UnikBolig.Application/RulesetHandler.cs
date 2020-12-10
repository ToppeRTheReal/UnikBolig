using System;
using System.Linq;
using UnikBolig.DataAccess;
using UnikBolig.Models;

namespace UnikBolig.Application
{
    public interface IRulesetHandler
    {
        public void Create(EstateRulesetModel ruleset, string Token);
    }

    public class RulesetHandler : IRulesetHandler
    {
        private readonly IDataAccess Context;

        public RulesetHandler(IDataAccess context)
        {
            this.Context = context;
        }

        public void Create(EstateRulesetModel ruleset, string Token)
        {
            var UserHandler = new UserHandler(this.Context);
            if (!UserHandler.AuthenticateUser(ruleset.UserID, Token))
                throw new Exception("Unauthorized");

            var user = Context.Users.Where(x => x.ID == ruleset.UserID).FirstOrDefault();
            if (user.Type != "landlord")
                throw new Exception("Unauthorized");

            ruleset.ID = Guid.NewGuid();
            this.Context.Rulesets.Add(ruleset);
            this.Context.SaveChanges();
        }
    }
}
