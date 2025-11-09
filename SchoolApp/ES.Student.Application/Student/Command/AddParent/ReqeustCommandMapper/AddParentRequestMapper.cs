using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Student.Application.Student.Command.AddStudents;

namespace ES.Student.Application.Student.Command.AddParent.ReqeustCommandMapper
{
    public static class AddParentRequestMapper
    {
        public static AddParentCommand ToCommand(this AddParentRequest request)
        {
            return new AddParentCommand(
               
                request.fullName,
                request.parentType,
                request.phoneNumber,
                request.email,
                request.address,
                request.occupation,
                request.imageUrl,
                request.createdBy,
                request.createdAt,
                request.modifiedBy,
                request.modifiedAt

            );
        }

    }
}
