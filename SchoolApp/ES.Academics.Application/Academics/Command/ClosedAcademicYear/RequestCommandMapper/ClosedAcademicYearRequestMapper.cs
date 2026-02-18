using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.ClosedAcademicYear.RequestCommandMapper
{
    public static class ClosedAcademicYearRequestMapper
    {
        public static ClosedAcademicYearCommand ToCommand(this ClosedAcademicYearRequest request)
        {
            return new ClosedAcademicYearCommand
                (
                request.closedAcademicId
                );
        }
    }
}
