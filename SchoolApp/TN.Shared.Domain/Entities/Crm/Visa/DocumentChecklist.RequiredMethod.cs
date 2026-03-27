using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.Entities.Crm.Visa
{
    public partial class DocumentChecklist
    {   
        public void Required(string user)
        {
            IsRequired = true;
            ModifiedBy = user;
            ModifiedAt = DateTime.UtcNow;
        }

        public void NonRequired(string user)
        {
            IsRequired = false;
            ModifiedBy = user;
            ModifiedAt = DateTime.UtcNow;
        }
    }
}
