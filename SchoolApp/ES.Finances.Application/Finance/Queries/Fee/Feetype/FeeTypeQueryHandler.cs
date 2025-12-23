using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.Feetype
{
    public class FeeTypeQueryHandler : IRequestHandler<FeeTypeQuery, Result<PagedResult<FeeTypeResponse>>>
    {
        private readonly IFeeTypeServices _feeTypeServices;
        private readonly IMapper _mapper;

        public FeeTypeQueryHandler(IFeeTypeServices feeTypeServices, IMapper mapper)
        {
            _feeTypeServices = feeTypeServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<FeeTypeResponse>>> Handle(FeeTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var results = await _feeTypeServices.FeeType(request.PaginationRequest, cancellationToken);
                var resultsDisplay = _mapper.Map<PagedResult<FeeTypeResponse>>(results.Data);
                return Result<PagedResult<FeeTypeResponse>>.Success(resultsDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
