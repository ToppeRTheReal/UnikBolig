using System;
using System.Collections.Generic;
using System.Linq;
using UnikBolig.DataAccess.Repository.Intefaces;
using UnikBolig.Models;

namespace UnikBolig.DataAccess.Repository
{
    public class TokenRepository : ITokenRepository
    {

        private DataAccess Context;

        public TokenRepository(DataAccess _Context)
        {
            this.Context = _Context;
        }

        public void Add(Guid UserID)
        {
            var user = Context.Users.Where(x => x.ID == UserID).FirstOrDefault();
            if (user == null)
                throw new Exception("User not found");

            var token = new TokenModel();
            token.ID = Guid.NewGuid();
            token.Token = RandomString(100);
            token.UserID = UserID;

            var tokenCheck = Context.Tokens.Where(x => x.UserID == UserID).FirstOrDefault();
            if (tokenCheck != null)
                Context.Tokens.Remove(tokenCheck);

            Context.Add(token);
        }

        public TokenModel GetTokenByUserID(Guid ID)
        {
            return Context.Tokens.Where(x => x.UserID == ID).FirstOrDefault();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void UpdateTokenFromUserID(Guid UserID)
        {
            var token = Context.Tokens.Where(x => x.UserID == UserID).FirstOrDefault();
            token.Token = RandomString(100);
        }

        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
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
