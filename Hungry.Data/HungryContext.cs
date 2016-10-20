using Hungry.Data.Migrations;
using Hungry.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace Hungry.Data
{
    public class HungryContext : IdentityDbContext
    {
        public HungryContext() : base("name=HungryContext")
        {
            Database.SetInitializer(
               new MigrateDatabaseToLatestVersion<HungryContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Subscribers)
                .WithMany()
                .Map(mapping =>
                {
                    mapping.MapLeftKey("userId")
                        .MapRightKey("subscriberId")
                        .ToTable("UserSubscribers");
                });
        }

        public virtual IDbSet<Recipe> Recipes { get; set; }

        public static HungryContext Create()
        {
            return new HungryContext();
        }
    }
}
