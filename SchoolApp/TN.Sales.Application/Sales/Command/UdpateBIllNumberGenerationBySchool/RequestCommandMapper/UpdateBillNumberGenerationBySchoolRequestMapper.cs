using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationBySchool.RequestCommandMapper
{
   public static class UpdateBillNumberGenerationBySchoolRequestMapper
    {
        public static UpdateBillNumberGenerationBySchoolCommand ToCommand(this UpdateBillNumberGenerationBySchoolRequest request, string schoolId)
        {
            return new UpdateBillNumberGenerationBySchoolCommand
            (
               
               request.BillNumberGenerationType, 
                schoolId
            );
        }
    }
}
