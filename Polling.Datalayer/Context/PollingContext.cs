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
        public DbSet<Group> Groups { get; set; }

        #endregion

        #region Vote

        public DbSet<Vote> Votes { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<UserVote> UsersVotes { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserVote>()
                .HasOne(uv => uv.Option)
                .WithMany(o => o.UserVotes)
                .HasForeignKey(uv => uv.OptionId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDelete);
            modelBuilder.Entity<Vote>().HasQueryFilter(u => !u.IsDelete);

            base.OnModelCreating(modelBuilder);
        }
    }
}
