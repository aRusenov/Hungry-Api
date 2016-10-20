using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hungry.Models
{
    public class User : IdentityUser
    {
        private ICollection<User> subscribers;
        private ICollection<Recipe> recipes;

        public User()
        {
            this.subscribers = new HashSet<User>();
            this.recipes = new HashSet<Recipe>();
        }

        public virtual ICollection<User> Subscribers
        {
            get { return this.subscribers; }
            set { this.subscribers = value; }
        }

        public virtual ICollection<Recipe> Recipes
        {
            get { return this.recipes; }
            set { this.recipes = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
    }
}
