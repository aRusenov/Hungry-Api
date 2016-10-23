using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hungry.Models
{
    public class Subscription
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        public string SubscriberId { get; set; }

        public virtual User Subscriber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
