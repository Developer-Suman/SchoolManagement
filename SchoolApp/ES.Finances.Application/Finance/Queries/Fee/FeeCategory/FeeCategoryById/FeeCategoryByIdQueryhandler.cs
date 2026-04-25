using AutoMapper;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureById;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Queries.Fee.FeeCategory.FeeCategoryById
{
    public class FeeCategoryByIdQueryhandler : IRequestHandler<FeeCategoryByIdQuery, Result<FeeCategoryByIdResponse>>
    {
        private readonly IFeeCategoryServices _feeCategoryServices;
        private readonly IMapper _mapper;

        public FeeCategoryByIdQueryhandler(IFeeCategoryServices feeCategoryServices, IMapper mapper)
        {
                _mapper = mapper;
            _feeCategoryServices = feeCategoryServices;
            
        }
        public async Task<Result<FeeCategoryByIdResponse>> Handle(FeeCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _feeCategoryServices.GetCategory(request.id);
                return Result<FeeCategoryByIdResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }
    }
}
