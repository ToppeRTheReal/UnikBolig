using System;
using Microsoft.EntityFrameworkCore;
using UnikBolig.Models;

namespace UnikBolig.DataAccess
{
    public class DataMock : IDataAccess
    {
        public UserModel Renter;
        public string RenterToken;
        public UserModel Landlord;
        public string LandlordToken;
        public EstateModel Estate;

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TokenModel> Tokens { get; set; }
        public DbSet<EstateModel> Estates { get; set; }
        public DbSet<EstateRulesetModel> Rulesets { get; set; }
        public DbSet<UserDetailModel> UserDetails { get; set; }
        public DbSet<WaitingList> WaitingList { get; set; }


        public DataMock()
        {
            CreateUsers();
            CreateEstates();
            AddToWaitingList();
        }

        public void AddToWaitingList()
        {
            WaitingList list = new WaitingList();
            list.ID = Guid.NewGuid();
            list.UserID = this.Renter.ID;
            list.EstateID = this.Estate.ID;
        }

        private void CreateEstates()
        {
            EstateRulesetModel ruleset = new EstateRulesetModel();
            ruleset.ID = Guid.NewGuid();
            ruleset.UserID = this.Landlord.ID;
            ruleset.Name = "Default ruleset";
            ruleset.Dog = false;
            ruleset.Cat = true;
            ruleset.Creep = false;
            ruleset.Fish = false;
            this.Rulesets.Add(ruleset);
            
            EstateModel estate = new EstateModel();
            estate.ID = Guid.NewGuid();
            estate.RulesetID = ruleset.ID;
            estate.UserID = this.Landlord.ID;
            estate.Name = "Flot bolig";
            estate.ImgUrl = "Dette er et url";
            estate.Size = 55;
            estate.StreetName = "Boessevej";
            estate.HouseNumber = 69;
            estate.Postal = 7100;
            this.Estates.Add(estate);
            this.Estate = estate;

        }

        private void CreateUsers()
        {
            this.Renter = new UserModel();
            this.Renter.ID = Guid.NewGuid();
            this.Renter.Email = "renter@unikbolig.dk";
            this.Renter.FirstName = "Renter";
            this.Renter.LastName = "1";
            this.Renter.Password = "123456";
            this.Renter.Phone = "12345678";
            this.Users.Add(this.Renter);

            this.RenterToken = "secrettoken";
            TokenModel UsrToken = new TokenModel();
            UsrToken.ID = Guid.NewGuid();
            UsrToken.UserID = Renter.ID;
            UsrToken.Token = this.RenterToken;
            this.Tokens.Add(UsrToken);

            UserDetailModel details = new UserDetailModel();
            details.ID = Guid.NewGuid();
            details.UserID = this.Renter.ID;
            details.About = "Hey jeg er en dummy :)";
            details.Dog = true;
            details.Cat = false;
            details.Creep = false;
            details.Fish = true;
            this.UserDetails.Add(details);

            this.Landlord.ID = Guid.NewGuid();
            this.Landlord.Email = "renter@unikbolig.dk";
            this.Landlord.FirstName = "Landlord";
            this.Landlord.LastName = "1";
            this.Landlord.Password = "123456";
            this.Landlord.Phone = "12345678";
            this.Users.Add(Landlord);

            var LandToken = new TokenModel();
            this.LandlordToken = "secrettoken";
            LandToken.ID = Guid.NewGuid();
            LandToken.UserID = this.Landlord.ID;
            LandToken.Token = this.LandlordToken;
            this.Tokens.Add(LandToken);
        }

        public void SaveChanges()
        {
            
        }
    }
}
