using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.AddSubject.RequestCommandMapper
{
    public static class AddSubjectRequestMapper
    {
        public static AddSubjectCommand ToCommand(this AddSubjectRequest request)
        {
            return new AddSubjectCommand
                (
                request.name,
                request.code,
                request.creditHours,
                request.description,
                request.classId
                );
        }
    }
}
