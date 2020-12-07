using System;
using Xunit;
using UnikBolig.Application;
using UnikBolig.Models;
using UnikBolig.DataAccess;
using System.Linq;

namespace Tests
{

    [Collection("Sequential")]
    public class UserTests
    {
        Guid UserID = Guid.NewGuid();
        UserHandler Handler = new UserHandler(null);

        [Fact]
        public void CreateUser()
        {
            this.Handler.Create(UserID, "Thomas", "Clausen", "topperhdoriginal@gmail.com", "40242041", "pwd");

            var Context = new DataAccess();
            var check = Context.Users.Where(x => x.ID == UserID).FirstOrDefault();

            Assert.NotNull(check);
        }

        [Fact]
        public void FindUser()
        {
            // Virker ikke og giver ingen mening
            var user = Handler.GetByID(UserID);
            Assert.NotNull(user);
        }

    }
}
