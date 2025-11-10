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
        string firstName="",
        string? middleName="",
        string lastName = "",
        string registrationNumber = "",
        GenderStatus genderStatus = default,
        StudentStatus studentStatus = default,
        DateTime dateOfBirth = default,
        string? email = "",
        string? phoneNumber = "",
        string? imageUrl="",
        string? address = "",
        DateTime enrollmentDate = default,
        string? parentId = "",
        string? classSectionId = "",
        int? provinceId = 0,
        int? districtId=0,
        int? wardNumber = 0
    );
}
