using System;
using UnikBolig.Models;
using UnikBolig.DataAccess;
using System.Linq;
using System.Transactions;

namespace UnikBolig.Application
{
    public interface IUserHandler
    {
        public void Create(Guid ID, string FirstName, string LastName, string Email, string Phone, string Password);
        public TokenModel Login(string Email, string Password);
        public UserModel GetByID(Guid ID, string Token);
        public bool AuthenticateUser(Guid UserID, string Token);
        public void ChangeUserType(Guid ID, string Token, string Type);
        public void CreateUpdateUserDetails(UserDetailModel Details, string token);
        public UserDetailModel GetDetails(Guid UserID, string Token);
    }

    public class UserHandler : IUserHandler
    {
        private readonly IDataAccess Context;

        public UserHandler(IDataAccess context)
        {
            this.Context = context;
        }

        public void Create(Guid ID, string FirstName, string LastName, string Email, string Phone, string Password)
        {
            if (FirstName == "" || LastName == "" || Email == "" || Phone == "" || Password == "")
                throw new Exception("Some values were empty");

            var check = this.Context.Users.Where(x => x.Email == Email || x.Phone == Phone).FirstOrDefault();
            if (check != null)
                throw new Exception("Email eller telefon er allerede i brug");

            UserModel User = new UserModel();
            User.ID = ID;
            User.FirstName = FirstName;
            User.LastName = LastName;
            User.Email = Email;
            User.Phone = Phone;
            User.Password = Password;
            User.Type = "renter";

            var user = Context.Users.Where(x => x.ID == ID).FirstOrDefault();
            if(user != null)
                throw new Exception("User Already Exists");

            TokenModel Token = new TokenModel();
            Token.ID = Guid.NewGuid();
            Token.Token = RandomString(100);
            Token.UserID = User.ID;

            using (TransactionScope scope = new TransactionScope())
            {
                Context.Users.Add(User);
                Context.SaveChanges();
                Context.Tokens.Add(Token);
                Context.SaveChanges();

                scope.Complete();
            }
        }

        public TokenModel Login(string Email, string Password)
        {
            if(Email == string.Empty || Password == string.Empty)
                throw new Exception("Username or password were left empty");

            var User = this.Context.Users.Where(x => x.Email == Email).FirstOrDefault();

            if (User == null)
                throw new Exception("User not found");

            if (User.Password != Password)
                throw new Exception("User not found");

            var Token = Context.Tokens.Where(x => x.UserID == User.ID).FirstOrDefault();
            Token.Token = RandomString(100);

            Context.SaveChanges();

            return Token;
        }

        public UserModel GetByID(Guid ID, string Token)
        {
            if (!this.AuthenticateUser(ID, Token))
                throw new Exception("Unauthorized");

            return this.Context.Users.Where(x => x.ID == ID).FirstOrDefault();
        }

        public bool AuthenticateUser(Guid UserID, string Token)
        {
            var _Token = this.Context.Tokens.Where(x => x.UserID == UserID).FirstOrDefault();
            if (_Token == null)
                throw new Exception("User not found");

            if (_Token.Token == Token)
                return true;
            else
                return false;
        }

        // Concurrency not added
        public void ChangeUserType(Guid ID, string Token, string Type)
        {
            if (Type != "renter" && Type != "landlord" && Type != "admin")
                throw new Exception("Usertype does not exist");

            if (!AuthenticateUser(ID, Token))
                throw new Exception("Unauthorized");
            try
            {
                var User = this.Context.Users.Where(x => x.ID == ID).FirstOrDefault();
                User.Type = Type;
                Context.SaveChanges();
            }catch (Exception e)
            {
                throw e;
            }
        }

        public void CreateUpdateUserDetails(UserDetailModel Details, string token)
        {
            var User = this.Context.Users.Where(x => x.ID == Details.UserID).FirstOrDefault();
            if (User == null)
                throw new Exception("User not found");

            if (!this.AuthenticateUser(Details.UserID, token))
                throw new Exception("Unauthorized");

            var _Details = this.Context.UserDetails.Where(x => x.UserID == Details.UserID).FirstOrDefault();
            if(_Details == null)
            {
                Details.ID = Guid.NewGuid();
                Context.UserDetails.Add(Details);
            }else
            {
                _Details.About = Details.About;
                _Details.Dog = Details.Dog;
                _Details.Cat = Details.Cat;
                _Details.Creep = Details.Creep;
                _Details.Fish = Details.Fish;
            }

            Context.SaveChanges();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public UserDetailModel GetDetails(Guid UserID, string Token)
        {
            if (!this.AuthenticateUser(UserID, Token))
                throw new Exception("Unauthorized");

            return this.Context.UserDetails.Where(x => x.UserID == UserID).FirstOrDefault();
        }
    }
}
