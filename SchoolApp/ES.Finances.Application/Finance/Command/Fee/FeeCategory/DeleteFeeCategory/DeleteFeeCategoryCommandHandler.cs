using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FeeCategory.FeeCategoryById;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.FeeCategory.DeleteFeeCategory
{
    public class DeleteFeeCategoryCommandHandler : IRequestHandler<DeleteFeeCategoryCommand, Result<bool>>
    {
        private readonly IFeeCategoryServices _feeCategoryServices;
        private readonly IMapper _mapper;

        public DeleteFeeCategoryCommandHandler(IMapper mapper,IFeeCategoryServices feeCategoryServices)
        {
            _mapper = mapper;
            _feeCategoryServices = feeCategoryServices;
        }

        public async Task<Result<bool>> Handle(DeleteFeeCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var delete = await _feeCategoryServices.Delete(request.id);
                if (delete is null)
                {
                    return Result<bool>.Failure("NotFound", "FeeCategory not Found");
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
