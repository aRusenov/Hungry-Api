using System;
using System.Collections.Generic;
using Hungry.Data.Repositories;
using Hungry.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Hungry.Data
{
    public class HungryData : IHungryData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;

        public HungryData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<Ingredient> Ingredients
        {
            get
            {
                return this.GetRepository<Ingredient>();
            }
        }

        public IRepository<RecipeIngredient> RecipeIngredients
        {
            get
            {
                return this.GetRepository<RecipeIngredient>();
            }
        }

        public IRepository<Recipe> Recipes
        {
            get
            {
                return this.GetRepository<Recipe>();
            }
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<Subscription> Subscriptions
        {
            get
            {
                return this.GetRepository<Subscription>();
            }
        }

        public IRepository<Activity> Activities
        {
            get
            {
                return this.GetRepository<Activity>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!this.repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof(GenericRepository<T>);
                var repository = Activator.CreateInstance(typeOfRepository, this.context);

                this.repositories.Add(type, repository);
            }

            return (IRepository<T>)this.repositories[type];
        }
    }
}
