using Hungry.Data.Repositories;
using Hungry.Models;

namespace Hungry.Data
{
    public interface IHungryData
    {
        IRepository<User> Users { get; }

        IRepository<Recipe> Recipes { get; }

        int SaveChanges();
    }
}
