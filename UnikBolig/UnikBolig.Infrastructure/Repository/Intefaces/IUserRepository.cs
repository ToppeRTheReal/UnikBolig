using System;
using System.Collections.Generic;
using UnikBolig.Models;

namespace UnikBolig.DataAccess.Repository.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        IEnumerable<UserModel> GetUsers();
        UserModel GetUserByID(Guid ID);
        void Add(UserModel User);
        void Delete(UserModel User);
        void Update(UserModel User);
        void Save();
    }
}
