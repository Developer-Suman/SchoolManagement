using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Shared.Application.Shared.Queries.GetFilterUserActivity
{
    public class GetFilterUserActivityQueryHandler : IRequestHandler<GetFilterUserActivityQuery, Result<PagedResult<GetFilterUserActivityResponse>>>
    {
        private readonly IUserActivity _userActivity;
        private readonly IMapper _mapper;
        public GetFilterUserActivityQueryHandler(IUserActivity userActivity, IMapper mapper)
        {
            _userActivity = userActivity;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<GetFilterUserActivityResponse>>> Handle(GetFilterUserActivityQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var userActivity = await _userActivity.GetFilterUserActivity(request.PaginationRequest, request.UserActivityDTOs);

                var userActivityResult = _mapper.Map<PagedResult<GetFilterUserActivityResponse>>(userActivity.Data);

                return Result<PagedResult<GetFilterUserActivityResponse>>.Success(userActivityResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterUserActivityResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
