using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureByClass;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeStructureByStudent
{
    public class FeeStructureByStudentQueryHandler : IRequestHandler<FeeStructureByStudentQuery, Result<FeeStructureByStudentResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IFeeStructureServices _feeStructureServices;

        public FeeStructureByStudentQueryHandler(IMapper mapper, IFeeStructureServices feeStructureServices)
        {
            _mapper = mapper;
            _feeStructureServices = feeStructureServices;

        }
        public async Task<Result<FeeStructureByStudentResponse>> Handle(FeeStructureByStudentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _feeStructureServices.FeeStructureByStudent(request.FeeStructureByStudentDTOs);

                var resultDisplay = _mapper.Map<FeeStructureByStudentResponse>(result.Data);

                return Result<FeeStructureByStudentResponse>.Success(resultDisplay);
            }
            catch (Exception ex)
            {
                return Result<FeeStructureByStudentResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
