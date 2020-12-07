
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnikBolig.Models;

namespace UnikBolig.Web.Models
{
    public class ConnectionStringClass : DbContext
    {

        public ConnectionStringClass(DbContextOptions<ConnectionStringClass> options) : base(options)
        {

        }

           
        public DbSet<UserDetailModel> UserDetails { get; set; }

    }
}
