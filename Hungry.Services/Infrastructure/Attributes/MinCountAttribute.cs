using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hungry.Services.Controllers
{
    public class MinCountAttribute : ValidationAttribute
    {
        private int minCount;

        public MinCountAttribute(int minCount)
        {
            this.MinCount = minCount;
        }

        public int MinCount
        {
            get { return this.minCount; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("MinCount cannot be negative");
                }

                this.minCount = value;
            }
        }

        public override bool IsValid(object value)
        {
            var collection = value as ICollection;
            if (collection != null)
            {
                return collection.Count >= this.MinCount;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("The {0} field requires at least {1} element(s)",
                name, this.MinCount);
        }
    }
}