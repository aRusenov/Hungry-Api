using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Quantity { get; set; }

        public Measurement Measurement { get; set; }
    }
}
