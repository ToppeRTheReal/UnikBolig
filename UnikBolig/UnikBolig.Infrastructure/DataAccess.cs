﻿using System;
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

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=176.20.223.184;Database=UnikBolig;User Id=Unik;Password=UnikBolig123");
        }

        void IDataAccess.SaveChanges()
        {
            this.SaveChanges();
        }
    }
}
