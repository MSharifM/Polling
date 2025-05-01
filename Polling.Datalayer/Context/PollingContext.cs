using Microsoft.EntityFrameworkCore;
using Polling.Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polling.Datalayer.Context
{
    public class PollingContext : DbContext
    {
        public PollingContext(DbContextOptions<PollingContext> options) : base(options)
        {
        }

        #region User

        public DbSet<User> Users { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDelete);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
