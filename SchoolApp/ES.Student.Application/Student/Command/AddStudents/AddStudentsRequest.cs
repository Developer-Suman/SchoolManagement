using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Command.AddStudents
{
    public record AddStudentsRequest
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
        string? address,
        DateTime enrollmentDate,
        string? parentId,
        string? classSectionId,
        int provinceId,
        int districtId,
        int wardNumber,
        int? vdcid,
        int? municipalityId,
        IFormFile? studentImg
        );
}
