using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hungry.Models
{
    public class Ingredient
    {
        private ICollection<RecipeIngredient> recipes;

        public Ingredient()
        {
            this.recipes = new HashSet<RecipeIngredient>();
        }

        public int Id { get; set; }

        [Required, MinLength(3), MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<RecipeIngredient> Recipes
        {
            get { return this.recipes; }
            set { this.recipes = value; }
        }
    }
}
