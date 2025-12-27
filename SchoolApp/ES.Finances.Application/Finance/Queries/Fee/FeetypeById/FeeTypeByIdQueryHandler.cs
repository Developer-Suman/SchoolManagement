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

namespace ES.Finances.Application.Finance.Queries.Fee.FeetypeById
{
    public class FeeTypeByIdQueryHandler : IRequestHandler<FeeTypeByIdQuery, Result<FeetypeByidResponse>>
    {
        private readonly IFeeTypeServices _feeTypeServices;
        private readonly IMapper _mapper;

        public FeeTypeByIdQueryHandler(IFeeTypeServices feeTypeServices, IMapper mapper)
        {
            _feeTypeServices = feeTypeServices;
            _mapper = mapper;

        }
        public async Task<Result<FeetypeByidResponse>> Handle(FeeTypeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var feeTypeById = await _feeTypeServices.GetFeetype(request.id);
                return Result<FeetypeByidResponse>.Success(feeTypeById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching by using id", ex);
            }
        }
    }
}
