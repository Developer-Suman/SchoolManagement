
using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.FilterConsultancyClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.ServiceInterface
{
    public interface IConsultancyClassServices
    {
        Task<Result<AddConsultancyClassResponse>> Add(AddConsultancyClassCommand addConsultancyClassCommand);
        Task<Result<PagedResult<FilterConsultancyClassResponse>>> Filter(PaginationRequest paginationRequest, FilterConsultancyClassDTOs filterConsultancyClassDTOs);
        Task<Result<PagedResult<ConsultancyClassResponse>>> All(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}
