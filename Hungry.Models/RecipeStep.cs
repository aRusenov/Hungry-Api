using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hungry.Models
{
    public class RecipeStep
    {
        public int Id { get; set; }

        public int Order { get; set; }

        [Required, MinLength(10), MaxLength(300)]
        public string Content { get; set; }

        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
