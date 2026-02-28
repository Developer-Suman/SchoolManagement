using ES.Academics.Application.Academics.Command.ClosedAcademicYear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.ServiceInterface
{
    public interface IStudentsPromotion
    {
        Task<Result<ClosedAcademicYearResponse>> CloseAcademicYear(ClosedAcademicYearCommand command);
        Task<string?> GetCurrentAcademicYear();

        Task<string?> PromoteStudentsBulk(string currentYearId, string nextYearId);
    }
}
