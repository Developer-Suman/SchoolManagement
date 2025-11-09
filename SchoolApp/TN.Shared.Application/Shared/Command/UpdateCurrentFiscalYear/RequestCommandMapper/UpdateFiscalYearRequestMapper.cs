using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear.RequestCommandMapper
{
    public static class UpdateFiscalYearRequestMapper
    {
        public static UpdateFiscalYearCommand ToCommand(this UpdateFiscalYearRequest request, string schoolId)
        {

            return new UpdateFiscalYearCommand
                (
                        schoolId,
                        request.currentFiscalYearId
                );
        }
    }
}
