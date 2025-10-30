using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Queries.GetAllStudents
{
    public  record GetAllStudentQueryResponse
    (

         string firstName,
        string? middleName,
        string lastName,
        string admissionNumber,
        GenderStatus genderStatus,
        StudentStatus studentStatus,
        DateTime dateOfBirth,
        string? email,
        string? phoneNumber,
        string? imageUrl,
        string? address,
        DateTime enrollmentDate,
        string? parentId,
        string? classSectionId

    );
}
