using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Academics.Application.Academics.Command.UpdateSubject.RequestCommandMapper
{
    public static class UpdateSubjectRequestMapper
    {
        public static UpdateSubjectCommand ToCommand(this  UpdateSubjectRequest request, string subjectId)
        {
            return new UpdateSubjectCommand(
                subjectId,
                request.name,
                request.code,
                request.creditHours,
                request.description,
                request.classId


                );
        }
    }
}
