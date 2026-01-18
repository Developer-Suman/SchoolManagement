using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureByClass
{
    public class FeeStructureByClassQueryHandler : IRequestHandler<FeeStructureByClassQuery, Result<PagedResult<FeeStructureByClassResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IFeeStructureServices _feeStructureServices;

        public FeeStructureByClassQueryHandler(IMapper mapper, IFeeStructureServices feeStructureServices)
        {
            _mapper = mapper;
            _feeStructureServices = feeStructureServices;
            
        }
        public async Task<Result<PagedResult<FeeStructureByClassResponse>>> Handle(FeeStructureByClassQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _feeStructureServices.getFeeStructureBy(request.paginationRequest, request.FeeStructureByClassDTOs);

                var resultDisplay = _mapper.Map<PagedResult<FeeStructureByClassResponse>>(result.Data);

                return Result<PagedResult<FeeStructureByClassResponse>>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FeeStructureByClassResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
