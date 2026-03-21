using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.TrainingRegistration.FilterTrainingRegistration
{
    public class FilterTrainingRegistrationQueryhandler : IRequestHandler<FilterTrainingRegistrationQuery, Result<PagedResult<FilterTrainingRegistrationResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ITrainingRegistrationServices _trainingRegistrationServices;

        public FilterTrainingRegistrationQueryhandler(IMapper mapper, ITrainingRegistrationServices trainingRegistrationServices)
        {
            _mapper = mapper;
            _trainingRegistrationServices = trainingRegistrationServices;
            
        }
        public async Task<Result<PagedResult<FilterTrainingRegistrationResponse>>> Handle(FilterTrainingRegistrationQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _trainingRegistrationServices.Filter(request.paginationRequest, request.FilterTrainingRegistrationDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FilterTrainingRegistrationResponse>>(result.Data);

                return Result<PagedResult<FilterTrainingRegistrationResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterTrainingRegistrationResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
