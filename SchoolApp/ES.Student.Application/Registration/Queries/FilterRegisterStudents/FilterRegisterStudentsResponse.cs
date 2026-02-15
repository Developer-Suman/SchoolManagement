using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Enum.SchoolEnrollment;

namespace ES.Student.Application.Registration.Queries.FilterRegisterStudents
{
    public record FilterRegisterStudentsResponse
    (
        string id,
            string studentId,
            string? classId,
            string academicYearId,
            EnrollmentStatus status,
            string schoolId,
            bool isActive,
             string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt
        );
}
