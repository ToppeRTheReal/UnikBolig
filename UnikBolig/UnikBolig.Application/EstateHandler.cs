using System;
using System.Linq;
using UnikBolig.Models;

namespace UnikBolig.Application
{
    public class EstateHandler
    {
        public void Create(EstateModel estate, string Token)
        {
            var userhandler = new UserHandler();
            if (!userhandler.AuthenticateUser(estate.UserID, Token))
                throw new Exception("Unauthorized");

            var Context = new DataAccess.DataAccess();
            var user = Context.Users.Where(x => x.ID == estate.UserID).FirstOrDefault();
            if (user == null || user.Type != "landlord")
                throw new Exception("Unauthorized");

            var ruleset = Context.Rulesets.Where(x => x.ID == estate.RulesetID).FirstOrDefault();
            if (ruleset == null)
                throw new Exception("ruleset not found");

            var houseCheck = Context.Estates.Where(x => x.StreetName == estate.StreetName && x.HouseNumber == estate.HouseNumber).FirstOrDefault();
            if (houseCheck != null)
                throw new Exception("House already exists");

            estate.ID = Guid.NewGuid();
            Context.Estates.Add(estate);
            Context.SaveChanges();
        }

        public EstateModel GetHouseByID(Guid EstateID)
        {
            var Context = new DataAccess.DataAccess();
            return Context.Estates.Where(x => x.ID == EstateID).FirstOrDefault();
        }
    }
}
