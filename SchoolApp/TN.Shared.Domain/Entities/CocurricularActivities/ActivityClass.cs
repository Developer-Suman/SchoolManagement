using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;

namespace TN.Shared.Domain.Entities.CocurricularActivities
{
    public class ActivityClass
    {
        public string ActivityId { get; set; }
        public Activity Activity { get; set; }

        public string ClassId { get; set; }
        public Class Class { get; set; }
    }
}
