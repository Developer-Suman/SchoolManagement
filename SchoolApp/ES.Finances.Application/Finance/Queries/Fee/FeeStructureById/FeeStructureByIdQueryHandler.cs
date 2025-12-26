using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureById
{
    public class FeeStructureByIdQueryHandler : IRequestHandler<FeeStructureByIdQuery, Result<FeeStructureByIdResponse>>
    {
        private IFeeStructureServices _feeStructureServices;
        private readonly IMapper _mapper;



        public FeeStructureByIdQueryHandler(IFeeStructureServices feeStructureServices, IMapper mapper)
        {
            _feeStructureServices = feeStructureServices;
            _mapper = mapper;

        }
        public async Task<Result<FeeStructureByIdResponse>> Handle(FeeStructureByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var feestructurebyId = await _feeStructureServices.GetFeeStructure(request.id);
                return Result<FeeStructureByIdResponse>.Success(feestructurebyId.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using id", ex);
            }
        }
    }
}
