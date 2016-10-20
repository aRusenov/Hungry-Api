using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hungry.Models
{
    public class RecipeIngredient
    {
        [Key, Column(Order = 0)]
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        [Key, Column(Order = 1)]
        public int IngredientId { get; set; }

        public virtual Ingredient Ingredient { get; set; }
    }
}
