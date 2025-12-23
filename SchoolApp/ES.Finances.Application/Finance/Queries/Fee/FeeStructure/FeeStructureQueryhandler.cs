using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructure
{
    public class FeeStructureQueryhandler : IRequestHandler<FeeStructureQuery, Result<PagedResult<FeeStructureResponse>>>
    {
        private readonly IFeeStructureServices _feeStructureServices;
        private readonly IMapper _mapper;

        public FeeStructureQueryhandler(IFeeStructureServices feeStructureServices, IMapper mapper)
        {
            _mapper = mapper;
            _feeStructureServices = feeStructureServices;


        }
        public async Task<Result<PagedResult<FeeStructureResponse>>> Handle(FeeStructureQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var results = await _feeStructureServices.FeeStructure(request.PaginationRequest, cancellationToken);
                var resultsDisplay = _mapper.Map<PagedResult<FeeStructureResponse>>(results.Data);
                return Result<PagedResult<FeeStructureResponse>>.Success(resultsDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
