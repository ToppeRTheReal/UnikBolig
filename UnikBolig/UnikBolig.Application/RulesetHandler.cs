using System;
using System.Collections.Generic;
using System.Linq;
using UnikBolig.DataAccess;
using UnikBolig.Models;

namespace UnikBolig.Application
{
    public interface IRulesetHandler
    {
        public void Create(EstateRulesetModel ruleset, string Token);
        public List<EstateRulesetModel> GetOwnedRuleset(Guid UserID, string Token);
    }

    public class RulesetHandler : IRulesetHandler
    {
        private readonly IDataAccess Context;
        IUserHandler userHandler;

        public RulesetHandler(IDataAccess context, IUserHandler h)
        {
            this.Context = context;
            this.userHandler = h;
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

        public List<EstateRulesetModel> GetOwnedRuleset(Guid UserID, string Token)
        {
            if (!this.userHandler.AuthenticateUser(UserID, Token))
                throw new Exception("Unauthorized");

            return this.Context.Rulesets.Where(x => x.UserID == UserID).ToList();
        }
    }
}
