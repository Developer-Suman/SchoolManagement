using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.District;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Setup.Application.Setup.Queries.AppNames
{
    public class AppNamesQueryhandler : IRequestHandler<AppNamesQuery, Result<PagedResult<AppNamesResponse>>>
    {
        private readonly IModule _moduleServices;
        private readonly IMapper _mapper;

        public AppNamesQueryhandler(IModule moduleServices, IMapper mapper)
        {
            _mapper = mapper;
            _moduleServices = moduleServices;

        }
        public async Task<Result<PagedResult<AppNamesResponse>>> Handle(AppNamesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = await _moduleServices.GatAppNames(request.PaginationRequest, cancellationToken);
                var queryDisplay = _mapper.Map<PagedResult<AppNamesResponse>>(query.Data);

                return Result<PagedResult<AppNamesResponse>>.Success(queryDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching", ex);
            }
        }
    }
}
