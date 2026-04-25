using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.DeleteFeeStructure
{
    public class DeleteFeeStructureCommandHandler : IRequestHandler<DeleteFeeStructureCommand, Result<bool>>
    {
        private readonly IFeeStructureServices _feeStructureServices;
        private readonly IMapper _mapper;

        public DeleteFeeStructureCommandHandler(IFeeStructureServices feeStructureServices, IMapper mapper)
        {
            _feeStructureServices = feeStructureServices;
            _mapper = mapper;
            
        }
        public async Task<Result<bool>> Handle(DeleteFeeStructureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var delete = await _feeStructureServices.Delete(request.id);
                if (delete is null)
                {
                    return Result<bool>.Failure("NotFound", "FeeStructure not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
