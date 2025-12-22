using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords.RequestCommandMapper
{
    public static class AddPaymentsRecordsRequestmapper
    {
        public static AddPaymentsRecordsCommand ToCommand(this AddpaymentsRecordsRequest request)
        {
            return new AddPaymentsRecordsCommand
                (
                request.studentfeeId,
                request.amountPaid,
                request.paymentDate,
                request.paymentMethod,
                request.reference
                );
        }
    }
}
