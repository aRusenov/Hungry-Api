using Hungry.Data.Repositories;
using Hungry.Models;
using System.Threading.Tasks;

namespace Hungry.Data
{
    public interface IHungryData
    {
        IRepository<User> Users { get; }

        IRepository<Recipe> Recipes { get; }

        IRepository<Ingredient> Ingredients { get; }

        IRepository<RecipeIngredient> RecipeIngredients { get; }

        IRepository<Subscription> Subscriptions { get; }

        IRepository<Activity> Activities { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
