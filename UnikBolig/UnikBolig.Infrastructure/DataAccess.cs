using System;
using Microsoft.EntityFrameworkCore;
using UnikBolig.Models;

namespace UnikBolig.DataAccess
{
    public class DataAccess : DbContext, IDataAccess
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<TokenModel> Tokens { get; set; }
        public DbSet<EstateModel> Estates { get; set; }
        public DbSet<EstateRulesetModel> Rulesets { get; set; }

        public DbSet<UserDetailModel> UserDetails { get; set; }
        public DbSet<WaitingList> WaitingList { get; set; }

        public DataAccess(DbContextOptions<DataAccess> access) : base(access)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=176.20.223.184;Database=UnikBolig;User Id=SA;Password=Dfdpea77nut");
        }


        void IDataAccess.SaveChanges()
        {
            try
            {
                this.SaveChanges();
            }catch(DbUpdateConcurrencyException)
            {
                throw new Exception("Values were updated elsewhere, please reload and try again");
            }
        }
    }
}
