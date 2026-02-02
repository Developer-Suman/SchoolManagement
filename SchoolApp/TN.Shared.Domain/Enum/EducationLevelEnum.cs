using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Enum
{
    public class EducationLevelEnum
    {
        public enum EducationLevel
        {
            [Display(Name = "+2 / Intermediate")]
            PlusTwoIntermediate = 1,

            [Display(Name = "Bachelor's")]
            Bachelors = 2,

            [Display(Name = "Master's")]
            Masters = 3
        }
    }
}
