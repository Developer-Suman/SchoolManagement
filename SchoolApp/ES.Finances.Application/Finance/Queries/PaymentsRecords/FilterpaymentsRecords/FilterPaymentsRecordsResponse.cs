using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Finance;

namespace ES.Finances.Application.Finance.Queries.PaymentsRecords.FilterpaymentsRecords
{
    public record FilterPaymentsRecordsResponse
   (
         string id,
            string studentfeeId,
            decimal amountPaid,
            DateTime paymentDate,
            decimal dueAmount,
            PaymentMethods paymentMethod,
            string? receiptNumber,
            string reference,
            bool isActive,
            string schoolId,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
