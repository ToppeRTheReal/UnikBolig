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
        DataMock Mock = new DataMock();
        UserHandler handler;

        public UserTests()
        {
            this.handler = new UserHandler(this.Mock);
        }

        [Fact]
        public void CreateUser()
        {
            Guid UserID = Guid.NewGuid();
            handler.Create(UserID, "Boesse", "Christian", "r@r.dk", "99999999", "pwd123");

            var usr = this.Mock.Users.Where(x => x.ID == UserID).FirstOrDefault();
            Console.WriteLine(usr.Email);
            Assert.NotNull(usr);
        }
    }
}
