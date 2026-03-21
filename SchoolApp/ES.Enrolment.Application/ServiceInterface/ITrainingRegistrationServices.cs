using ES.Enrolment.Application.Enrolments.Command.ConsultancyClass;
using ES.Enrolment.Application.Enrolments.Command.TranningRegistration.AddTranningRegistration;
using ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.ServiceInterface
{
    public interface ITrainingRegistrationServices
    {
        Task<Result<AddTranningRegistrationResponse>> Add(AddTranningRegistrationCommand addTranningRegistrationCommand);
        Task<Result<PagedResult<FilterTrainingRegistrationResponse>>> Filter(PaginationRequest paginationRequest, FilterTrainingRegistrationDTOs filterTrainingRegistrationDTOs);
       
    }
}
