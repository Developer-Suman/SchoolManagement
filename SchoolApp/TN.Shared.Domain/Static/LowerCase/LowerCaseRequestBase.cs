using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Static.LowerCase
{
    public abstract class LowerCaseRequestBase
    {
        public void Normalize()
        {
            var stringProperties = GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(string));

            foreach (var prop in stringProperties)
            {
                var currentValue = prop.GetValue(this) as string;
                if (!string.IsNullOrWhiteSpace(currentValue))
                {
                    prop.SetValue(this, currentValue.ToLower());
                }
            }
        }
    }
}
