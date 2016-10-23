using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hungry.Services.Models.Binding
{
    public class GetFeedBindingModel
    {
        [Range(5, 10)]
        public int Count { get; set; }

        public int Page { get; set; }
    }
}