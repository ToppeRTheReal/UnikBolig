using System;
using System.Linq;
using UnikBolig.Models;

namespace UnikBolig.Application
{
    public class RulesetHandler
    {
        public void Create(EstateRulesetModel ruleset, string Token)
        {
            var UserHandler = new UserHandler();
            if (!UserHandler.AuthenticateUser(ruleset.UserID, Token))
                throw new Exception("Unauthorized");

            var Context = new DataAccess.DataAccess();
            var user = Context.Users.Where(x => x.ID == ruleset.UserID).FirstOrDefault();
            if (user.Type != "landlord")
                throw new Exception("Unauthorized");

            ruleset.ID = Guid.NewGuid();
            Context.Rulesets.Add(ruleset);
            Context.SaveChanges();
        }
    }
}
