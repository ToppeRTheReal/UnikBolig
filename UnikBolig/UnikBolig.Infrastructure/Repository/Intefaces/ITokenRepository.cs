using System;
using System.Collections.Generic;
using UnikBolig.Models;

namespace UnikBolig.DataAccess.Repository.Intefaces
{
    public interface ITokenRepository : IDisposable
    {
        TokenModel GetTokenByUserID(Guid ID);
        void Add(Guid UserID);
        void UpdateTokenFromUserID(Guid UserID);
        void Save();
    }
}
