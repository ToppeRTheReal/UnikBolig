using System;
using System.Linq;
using UnikBolig.DataAccess;
using UnikBolig.Models;

namespace UnikBolig.Application
{
    public interface IEstateHandler
    {
        public void Create(EstateModel estate, string Token);
        public void Update(Guid EstateID, EstateModel estate, Guid UserID, string Token);
        public EstateModel GetByID(Guid EstateID);
    }

    public class EstateHandler : IEstateHandler
    {
        IDataAccess Context;

        public EstateHandler(IDataAccess context)
        {
            if (context == null)
                context = new DataAccess.DataAccess();

            this.Context = context;
        } 

        public void Create(EstateModel estate, string Token)
        {

            var userhandler = new UserHandler(this.Context);
            if (!userhandler.AuthenticateUser(estate.UserID, Token))
                throw new Exception("Unauthorized");

            var user = this.Context.Users.Where(x => x.ID == estate.UserID).FirstOrDefault();
            if (user == null || user.Type != "landlord")
                throw new Exception("Unauthorized");

            var ruleset = Context.Rulesets.Where(x => x.ID == estate.RulesetID).FirstOrDefault();
            if (ruleset == null)
                throw new Exception("ruleset not found");

            var houseCheck = Context.Estates.Where(x =>
                x.StreetName == estate.StreetName
                && x.HouseNumber == estate.HouseNumber
                && x.Floor == estate.Floor
            ).FirstOrDefault();

            if (houseCheck != null)
                throw new Exception("House already exists");

            estate.ID = Guid.NewGuid();
            Context.Estates.Add(estate);
            this.Context.SaveChanges();
        }

        public void Update(Guid EstateID, EstateModel estate, Guid UserID, string Token)
        {
            var Context = new DataAccess.DataAccess();
            var Estate = Context.Estates.Where(x => x.ID == estate.ID).FirstOrDefault();
            if (Estate == null)
                throw new Exception("Estate not found");

            if (Estate.UserID != UserID)
                throw new Exception("Unauthorized");

            var _UserHandler = new UserHandler();
            if (!_UserHandler.AuthenticateUser(UserID, Token))
                throw new Exception("Unauthorized");

            var Ruleset = Context.Rulesets.Where(x => x.ID == estate.RulesetID).FirstOrDefault();
            if (Ruleset == null)
                throw new Exception("Ruleset not found");

            if (Ruleset.UserID != estate.UserID)
                throw new Exception("You do not own that ruleset");

            Estate.Name = estate.Name;
            Estate.RulesetID = estate.RulesetID;
            Estate.Description = estate.Description;
            Estate.Size = estate.Size;
            Estate.StreetName = estate.StreetName;
            Estate.HouseNumber = estate.HouseNumber;
            Estate.Floor = estate.Floor;

            Context.SaveChanges();

        }

        public EstateModel GetByID(Guid EstateID)
        {
            var Context = new DataAccess.DataAccess();
            return Context.Estates.Where(x => x.ID == EstateID).FirstOrDefault();
        }
    }
}
