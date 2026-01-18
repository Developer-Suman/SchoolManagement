using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.Authentication.Queries.FilterUserByDate
{
   public class FilterUserByDateQueryHandler:IRequestHandler<FilterUserByDateQuery,Result<PagedResult<FilterUserByDateQueryResponse>>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;

        public FilterUserByDateQueryHandler(IUserServices userServices,IMapper mapper)
        {
            _userServices=userServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<FilterUserByDateQueryResponse>>> Handle(FilterUserByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterUser = await _userServices.GetUserFilter(request.paginationRequest, request.FilterUserDTOs, cancellationToken);

                if (!filterUser.IsSuccess || filterUser.Data == null)
                {
                    return Result<PagedResult<FilterUserByDateQueryResponse>>.Failure(filterUser.Message);
                }

                var filterUserResult = _mapper.Map<PagedResult<FilterUserByDateQueryResponse>>(filterUser.Data);

                return Result<PagedResult<FilterUserByDateQueryResponse>>.Success(filterUserResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterUserByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching User  by date: {ex.Message}");
            }
        }
    }
}
