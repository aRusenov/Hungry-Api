using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hungry.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        public int SourceId { get; set; }

        public ActivityType Type { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
