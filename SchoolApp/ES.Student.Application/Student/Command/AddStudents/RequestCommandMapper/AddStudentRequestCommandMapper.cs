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
                request.registrationNumber,
                request.genderStatus,
                request.studentStatus,
                request.dateOfBirth,
                request.email,
                request.phoneNumber,
                request.address,
                request.enrollmentDate,
                request.parentId,
                request.classSectionId,
                request.provinceId,
                request.districtId,
                request.wardNumber,
                request.vdcId,
                request.municipalityId,
                request.studentImg,
                request.classId
                );
        }
    }
}
