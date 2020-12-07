using System;
using Microsoft.EntityFrameworkCore;
using UnikBolig.Models;

namespace UnikBolig.DataAccess
{
    public interface IDataAccess
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<TokenModel> Tokens { get; set; }
        public DbSet<EstateModel> Estates { get; set; }
        public DbSet<EstateRulesetModel> Rulesets { get; set; }

        public DbSet<UserDetailModel> UserDetails { get; set; }
        public DbSet<WaitingList> WaitingList { get; set; }

        public void SaveChanges();
    }
}
