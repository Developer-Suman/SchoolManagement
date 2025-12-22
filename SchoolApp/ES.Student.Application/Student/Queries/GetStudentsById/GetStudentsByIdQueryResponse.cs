using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Students;

namespace ES.Student.Application.Student.Queries.GetStudentsById
{
    public record  GetStudentsByIdQueryResponse
    (

        string firstName="",
            string? middleName="",
            string lastName="",
            string registrationNumber="",
            GenderStatus gender=default,
            StudentStatus status=default,
            DateTime dateOfBirth=default,
            string? email="",
            string? phoneNumber="",
            string? studentImg= "",
            string? address="",
            DateTime enrollmentDate=default,
            string? parentId="",
            string? classSectionId="",
            int provinceId=0,
            int districtId=0,
            int wardNumber=0,
            string createdBy="",
            DateTime createdAt=default,
            string modifiedBy="",

            DateTime modifiedAt=default,
            string schoolId="",
            bool isActive=false,
            int? vdcid=0,
            int? municipalityId=0,
            string? classId=""

    );
}
