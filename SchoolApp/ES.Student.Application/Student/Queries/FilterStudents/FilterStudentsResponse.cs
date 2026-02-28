using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Enum;
using static TN.Shared.Domain.Enum.GenderEnum;
using static TN.Shared.Domain.Enum.SchoolEnrollment;

namespace ES.Student.Application.Student.Queries.FilterStudents
{
    public record FilterStudentsResponse
    (
        string id = "",
         string firstName = "",
        string? middleName = "",
        string lastName = "",
        string registrationNumber = "",
        Gender? genderStatus = default,
        StudentStatus studentStatus = default,
        DateTime dateOfBirth = default,
        string? email = "",
        string? phoneNumber = "",
        string? imageUrl = "",
        string? address = "",
        DateTime enrollmentDate = default,
        string? parentId = "",
        string? classSectionId = "",
        string? classId="",
        EnrollmentStatus? enrollmentStatus = default
        );
}
