using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hungry.Models
{
    public class Recipe
    {
        private ICollection<Ingredient> ingredients;

        public Recipe()
        {
            this.ingredients = new HashSet<Ingredient>();
        }

        public int Id { get; set; }

        [Required, MinLength(6), MaxLength(30)]
        public string Title { get; set; }

        [Required, MinLength(10), MaxLength(1000)]
        public string Content { get; set; }

        [Required]
        public TimeSpan Preparation { get; set; }

        public Difficulty Difficulty { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Ingredient> Ingredients
        {
            get { return this.ingredients; }
            set { this.ingredients = value; }
        }
    }
}
