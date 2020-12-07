using System;
using System.Collections.Generic;
using System.Linq;
using UnikBolig.DataAccess;
using UnikBolig.Models;

namespace UnikBolig.Application
{
    public interface IHousingHandler
    {
        public void Create(WaitingList list, string Token);
        public void Remove(Guid UserID, string Token, Guid EstateID);
        public List<EstateModel> GetAllHousingsWrittenUpFor(Guid UserID, string Token);
    }

    public class HousingHandler : IHousingHandler
    {

        IDataAccess Context;
        IUserHandler UserHandler;

        public HousingHandler(IDataAccess context)
        {
            if (context == null)
                context = new DataAccess.DataAccess();

            this.Context = context;
            this.UserHandler = new UserHandler(context);
        }

        public void Create(WaitingList list, string Token)
        {
            if (!this.UserHandler.AuthenticateUser(list.UserID, Token))
                throw new Exception("Unauthorized");

            list.ID = Guid.NewGuid();

            var User = this.Context.Users.Where(x => x.ID == list.UserID).FirstOrDefault();
            if (User == null)
                throw new Exception("User not found");

            var Estate = this.Context.Estates.Where(x => x.ID == list.EstateID).FirstOrDefault();
            if (Estate == null)
                throw new Exception("Estate not found");

            this.Context.WaitingList.Add(list);
            this.Context.SaveChanges();
        }

        public void Remove(Guid UserID, string Token, Guid EstateID)
        {
            if (!this.UserHandler.AuthenticateUser(UserID, Token))
                throw new Exception("Unauthorized");

            var waitingItem = this.Context.WaitingList.Where(x => x.UserID == UserID && x.EstateID == EstateID).FirstOrDefault();
            this.Context.WaitingList.Remove(waitingItem);
            this.Context.SaveChanges();
        }

        public List<EstateModel> GetAllHousingsWrittenUpFor(Guid UserID, string Token)
        {
            if (!this.UserHandler.AuthenticateUser(UserID, Token))
                throw new Exception("Unauthorized");

            List<WaitingList> items = this.Context.WaitingList.Where(x => x.UserID == UserID).ToList();

            List<EstateModel> HousesWrittenUpFor = new List<EstateModel>();
            foreach(var item in items)
            {
                var x = this.Context.Estates.Where(x => x.ID == item.EstateID).FirstOrDefault();

                if(x != null)
                    HousesWrittenUpFor.Add(x);
            }

            return HousesWrittenUpFor;
        }

        public List<UserWithPoints> GetHousingQualifiers(Guid EstateID, Guid UserID, string Token)
        {
            if (!this.UserHandler.AuthenticateUser(UserID, Token))
                throw new Exception("Unauthorized");

            var estate = this.Context.Estates.Where(x => x.ID == EstateID).FirstOrDefault();
            if (estate.UserID != UserID)
                throw new Exception("Unauthorized");

            var waitingListItems = this.Context.WaitingList.Where(x => x.EstateID == EstateID).ToList();
            var ruleset = this.Context.Rulesets.Where(x => x.ID == estate.RulesetID).FirstOrDefault();

            List<UserWithPoints> response = new List<UserWithPoints>();
            foreach(var item in waitingListItems)
            {
                UserWithPoints user = new UserWithPoints();
                var UserEntity = this.Context.Users.Where(x => x.ID == item.UserID).FirstOrDefault();
                UserDetailModel userDetails = this.Context.UserDetails.Where(x => x.UserID == item.UserID).FirstOrDefault();
                if (ruleset.Dog == userDetails.Dog)
                    user.Points++;
                if (ruleset.Cat == userDetails.Cat)
                    user.Points++;
                if (ruleset.Fish == userDetails.Fish)
                    user.Points++;
                if (ruleset.Creep == userDetails.Creep)
                    user.Points++;

                user.FirstName = UserEntity.FirstName;
                user.LastName = UserEntity.LastName;
                user.Email = UserEntity.Email;
                user.Phone = UserEntity.Phone;
                user.Type = UserEntity.Type;
                user.About = userDetails.About;

                response.Add(user);
            }
            response.OrderBy(usr => usr.Points);
            return response;
        }

        public class UserWithPoints : UserModel
        {
            public int Points { get; set; }
            public string About { get; set; }
        } 
    }
}
