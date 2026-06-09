using ES.Crm.Finance.Application.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Crm.Finance;
using static TN.Shared.Domain.Enum.CrmEnum;

namespace ES.Crm.Finance.Infrastructure.ServiceImpl
{
    public class CalculationServices : ICalculationServices
    {
        public decimal PaidAmount(Invoice invoice)
        => invoice.Payments?
            .Where(p => p.PaymentStatus == PaymentStatus.Completed)
            .Sum(p => p.Amount) ?? 0;

        public decimal DueAmount(Invoice invoice)
            => invoice.TotalAmount - PaidAmount(invoice);
    }
}
