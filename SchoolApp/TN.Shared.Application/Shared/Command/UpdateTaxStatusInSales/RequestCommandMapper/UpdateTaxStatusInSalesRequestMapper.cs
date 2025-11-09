using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales.RequestCommandMapper
{
    public static class UpdateTaxStatusInSalesRequestMapper
    {
        public static UpdateTaxStatusInSalesCommand ToCommand(this UpdateTaxStatusInSalesRequest request, string schoolId) 
        {
            return new UpdateTaxStatusInSalesCommand
                (
                    schoolId,
                    request.showTaxInSales
                
                );
        }
    }
}
