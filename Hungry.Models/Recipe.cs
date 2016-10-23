using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hungry.Models
{
    public class Recipe
    {
        private ICollection<RecipeIngredient> ingredients;
        private ICollection<RecipeStep> steps;

        public Recipe()
        {
            this.ingredients = new HashSet<RecipeIngredient>();
            this.steps = new HashSet<RecipeStep>();
        }

        public int Id { get; set; }

        [Required, MinLength(6), MaxLength(30)]
        public string Title { get; set; }

        [Required, MinLength(10), MaxLength(100)]
        public string Description { get; set; }

        [Required, Url]
        public string PreviewImageUrl { get; set; }

        [Required]
        public TimeSpan Preparation { get; set; }

        public Difficulty Difficulty { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<RecipeIngredient> RecipeIngredients
        {
            get { return this.ingredients; }
            set { this.ingredients = value; }
        }

        public virtual ICollection<RecipeStep> Steps
        {
            get { return this.steps; }
            set { this.steps = value; }
        }
    }
}
