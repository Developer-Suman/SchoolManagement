using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Enum;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Student.Application.Student.Queries.GetStudentByClass
{
    public record GetStudentByClassResponse
    (
        string id = "",
         string firstName = "",
        string? middleName = "",
        string lastName = "",
        string admissionNumber = "",
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
        string? classId = ""
        );
}
