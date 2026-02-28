using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.Enum;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Student.Application.Student.Command.AddStudents
{
    public record AddStudentsCommand
    (
        string? firstName,
        string? middleName,
        string? lastName,
        string? registrationNumber,
        Gender? genderStatus,
        StudentStatus studentStatus,
        DateTime dateOfBirth,
        string? email,
        string? phoneNumber,
        string? address,
        DateTime enrollmentDate,
        string? parentId,
        string? classSectionId,
        int? provinceId,
        int? districtId,
        int? wardNumber,
            int? vdcid,
        int? municipalityId,
        IFormFile? StudentsImg,
        string? classId


        ) : IRequest<Result<AddStudentsResponse>>;
}
