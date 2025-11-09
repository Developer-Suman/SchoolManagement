using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Command.AddStudents
{
    public record AddStudentsResponse
    (
        string firstName,
        string? middleName,
        string lastName,
        string registrationNumber,
        GenderStatus genderStatus,
        StudentStatus studentStatus,
        DateTime dateOfBirth,
        string? email,
        string? phoneNumber,
        string? imageUrl,
        string? address,
        DateTime enrollmentDate,
        string? parentId,
        string? classSectionId,
        int? provinceId,
        int? districtId,
        int? wardNumber
    );
}
