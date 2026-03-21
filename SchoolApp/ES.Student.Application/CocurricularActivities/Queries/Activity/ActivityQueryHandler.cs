using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.CocurricularActivities.Queries.Activity
{
    public class ActivityQueryHandler : IRequestHandler<ActivityQuery, Result<PagedResult<ActivityResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly ICocurricularActivityServices _cocurricularActivityServices;

        public ActivityQueryHandler(IMapper mapper,ICocurricularActivityServices cocurricularActivityServices)
        {
            _cocurricularActivityServices = cocurricularActivityServices;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<ActivityResponse>>> Handle(ActivityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _cocurricularActivityServices.AllActivity(request.PaginationRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
