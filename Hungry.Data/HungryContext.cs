using Hungry.Data.Migrations;
using Hungry.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace Hungry.Data
{
    public class HungryContext : IdentityDbContext<User>
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
                .WithRequired(s => s.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SubscribedTo)
                .WithRequired(s => s.Subscriber)
                .WillCascadeOnDelete(false);
        }

        public virtual IDbSet<Recipe> Recipes { get; set; }

        public virtual IDbSet<Ingredient> Ingredients { get; set; }

        public virtual IDbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public virtual IDbSet<Subscription> Subscriptions { get; set; }

        public virtual IDbSet<Activity> Activities { get; set; }

        public static HungryContext Create()
        {
            return new HungryContext();
        }
    }
}
