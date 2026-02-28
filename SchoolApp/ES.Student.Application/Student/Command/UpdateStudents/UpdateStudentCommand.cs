using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Students;
using static TN.Shared.Domain.Enum.GenderEnum;

namespace ES.Student.Application.Student.Command.UpdateStudents
{
    public record  UpdateStudentCommand
    (
          string id,
         string firstName,
        string? middleName,
        string lastName,
        string registrationNumber,
        Gender genderStatus,
        StudentStatus studentStatus,
        DateTime dateOfBirth,
        string? email,
        string? phoneNumber,
        string? imageUrl,
        string? address,
        DateTime enrollmentDate,
        string? parentId,
        string? classSectionId,
           int provinceId,
   int districtId,
   int wardNumber

    ) :IRequest<Result<UpdateStudentResponse>>;
}
