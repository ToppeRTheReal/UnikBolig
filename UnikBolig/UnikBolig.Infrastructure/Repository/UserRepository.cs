using System;
using System.Collections.Generic;
using UnikBolig.DataAccess.Repository.Interfaces;
using UnikBolig.Models;
using System.Linq;

namespace UnikBolig.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private DataAccess Context;
        private bool IsDisposed = false;

        public UserRepository(DataAccess data)
        {
            this.Context = data;
        }

        public void Add(UserModel User)
        {
            var check = this.Context.Users.Where(x => x.Email == User.Email || x.Phone == User.Phone).FirstOrDefault();
            if (check != null)
                throw new Exception("User already exists");

            Context.Users.Add(User);
            Context.SaveChanges();
            var TokenRepo = new TokenRepository(this.Context);
            TokenRepo.Add(User.ID);
        }

        public void Delete(UserModel User)
        {
            var check = this.Context.Users.Where(x => x.Email == User.Email).FirstOrDefault();
            if (check == null)
                throw new Exception("User not found");

            var token = Context.Tokens.Where(x => x.UserID == User.ID).FirstOrDefault();
            if (token == null)
                throw new Exception("Token not found");

            Context.Users.Remove(User);
            Context.Tokens.Remove(token);
        }

        public UserModel GetUserByEmail(string Email)
        {
            var User = Context.Users.Where(x => x.Email == Email).FirstOrDefault();
            return User;
        }

        public UserModel GetUserByID(Guid ID)
        {
            return Context.Users.Where(x => x.ID == ID).FirstOrDefault();
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return Context.Users.ToList<UserModel>();
        }

        public void Update(UserModel User)
        {
            var ContextUser = Context.Users.Where(x => x.ID == User.ID).FirstOrDefault();
            if (ContextUser == null)
                throw new Exception("User not found");

            var EmailCheck = Context.Users.Where(x => x.Email == User.Email).ToList();
            if(EmailCheck.Count() > 0)
            {
                if (EmailCheck[0] != ContextUser)
                    throw new Exception("Email already in use");
            }

            var PhoneCheck = Context.Users.Where(x => x.Phone == User.Phone).ToList();
            if (PhoneCheck.Count() > 0)
            {
                if (PhoneCheck[0] != ContextUser)
                    throw new Exception("Phone already in use");
            }


            ContextUser.FirstName = User.FirstName;
            ContextUser.LastName = User.LastName;
            ContextUser.Email = User.Email;
            ContextUser.Phone = User.Phone;
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if(!this.IsDisposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
