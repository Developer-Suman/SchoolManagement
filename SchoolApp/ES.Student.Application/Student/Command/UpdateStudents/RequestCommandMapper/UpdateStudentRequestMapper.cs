using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.UpdateStudents.RequestCommandMapper
{
    public static class UpdateStudentRequestMapper
    {
        public static UpdateStudentCommand ToUpdateStudentCommand(this UpdateStudentRequest request, string id)
        {
            return new UpdateStudentCommand(
                    id,
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
