using System;
using UnikBolig.Models;
using UnikBolig.DataAccess.Repository;

namespace UnikBolig.Application
{
    public class UserHandler
    {
        public void CreateUser(Guid ID, string FirstName, string LastName, string Email, string Phone, string Password)
        {
            if (FirstName == "" || LastName == "" || Email == "" || Phone == "" || Password == "")
                throw new Exception("Some values were empty");

            UserModel User = new UserModel();
            User.ID = ID;
            User.FirstName = FirstName;
            User.LastName = LastName;
            User.Email = Email;
            User.Phone = Phone;
            User.Password = Password;

            var UserRepo = new UserRepository(new DataAccess.DataAccess());
            try
            {
                UserRepo.Add(User);
                UserRepo.Save();
            }catch(Exception e)
            {
                throw e;
            }
        }

        public TokenModel Login(string Email, string Password)
        {
            if(Email == string.Empty || Password == string.Empty)
                throw new Exception("Username or password were left empty");

            UserRepository Repo = new UserRepository(new DataAccess.DataAccess());
            var User = Repo.GetUserByEmail(Email);

            if (User == null)
                throw new Exception("User not found");

            var TokenRepo = new TokenRepository(new DataAccess.DataAccess());
            TokenRepo.UpdateTokenFromUserID(User.ID);
            TokenRepo.Save();

            TokenModel Token = TokenRepo.GetTokenByUserID(User.ID);

            return Token;
        }

        public UserModel GetUserByID(Guid ID)
        {
            UserRepository Repo = new UserRepository(new DataAccess.DataAccess());
            return Repo.GetUserByID(ID);
        }

        public void DeleteUser(Guid UserID, Guid AdminID)
        {
            UserRepository Repo = new UserRepository(new DataAccess.DataAccess());

            // Add admin authentication
            UserModel User = Repo.GetUserByID(UserID);
            Repo.Delete(User);
            Repo.Save();
        }
    }
}
