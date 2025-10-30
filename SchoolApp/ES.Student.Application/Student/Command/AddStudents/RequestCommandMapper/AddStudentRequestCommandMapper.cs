using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.AddStudents.RequestCommandMapper
{
    public static class AddStudentRequestCommandMapper
    {
        public static AddStudentsCommand ToCommand(this AddStudentsRequest request)
        {
            return new AddStudentsCommand(
                request.firstName,
                request.middleName,
                request.lastName,
                request.admissionNumber,
                request.genderStatus,
                request.studentStatus,
                request.dateOfBirth,
                request.email,
                request.phoneNumber,
                request.imageUrl,
                request.address,
                request.enrollmentDate,
                request.parentId,
                request.classSectionId
                );
        }
    }
}
