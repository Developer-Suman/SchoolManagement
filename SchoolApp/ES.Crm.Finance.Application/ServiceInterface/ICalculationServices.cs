using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Finance;

namespace ES.Crm.Finance.Application.ServiceInterface
{
    public interface ICalculationServices
    {
        decimal PaidAmount(Invoice invoice);
        decimal DueAmount(Invoice invoice);
    }
}
