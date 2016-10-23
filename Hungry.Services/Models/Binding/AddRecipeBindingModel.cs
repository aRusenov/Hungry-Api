using Hungry.Models;
using Hungry.Services.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hungry.Services.Models.Binding
{
    public class AddRecipeBindingModel
    {
        [Required, MinLength(6)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required, MinCount(1)]
        public IngredientBindingModel[] Ingredients { get; set; }

        [Required, MinCount(1)]
        public RecipeStepBindingModel[] Steps { get; set; }
    }

    public class RecipeStepBindingModel
    {
        public int Order { get; set; }

        [Required]
        public string Description { get; set; }
    }

    public class IngredientBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Quantity { get; set; }

        public Measurement Measurement { get; set; }
    }
}