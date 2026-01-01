using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.DeleteFeeType
{
    public class DeleteFeeTypeCommandhandler : IRequestHandler<DeleteFeeTypeCommand, Result<bool>>
    {
        private readonly IFeeTypeServices _feeTypeServices;
        private readonly IMapper _mapper;

        public DeleteFeeTypeCommandhandler(IFeeTypeServices feeTypeServices, IMapper mapper)
        {
            _mapper = mapper;
            _feeTypeServices = feeTypeServices;
            
        }
        public async Task<Result<bool>> Handle(DeleteFeeTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var delete = await _feeTypeServices.Delete(request.id);
                if (delete is null)
                {
                    return Result<bool>.Failure("NotFound", "FeeType not Found");
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
