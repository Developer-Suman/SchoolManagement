using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.TotalFeeDetails
{
    public class TotalFeeDetailsQueryHandler : IRequestHandler<TotalFeeDetailsQuery, Result<TotalFeeDetailsResponse>>
    {
        private readonly IStudentFeeServices _studentfeeServices;
        private readonly IMapper _mapper;

        public TotalFeeDetailsQueryHandler(IStudentFeeServices studentfeeServices, IMapper mapper)
        {
            _studentfeeServices = studentfeeServices;
            _mapper = mapper;
        }
        public async Task<Result<TotalFeeDetailsResponse>> Handle(TotalFeeDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _studentfeeServices.GetTotalFeeDetails();

                var resultDisplay = _mapper.Map<TotalFeeDetailsResponse>(result.Data);

                return Result<TotalFeeDetailsResponse>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
