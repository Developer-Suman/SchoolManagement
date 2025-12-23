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

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFee
{
    public class StudentFeeQueryhandler : IRequestHandler<StudentFeeQuery, Result<PagedResult<StudentFeeResponse>>>
    {

        private readonly IStudentFeeServices _studentFeeServices;
        private readonly IMapper _mapper;

        public StudentFeeQueryhandler(IStudentFeeServices studentFeeServices, IMapper mapper)
        {
            _mapper = mapper;
            _studentFeeServices = studentFeeServices;


        }
        public async Task<Result<PagedResult<StudentFeeResponse>>> Handle(StudentFeeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var results = await _studentFeeServices.StudentFee(request.PaginationRequest, cancellationToken);
                var resultsDisplay = _mapper.Map<PagedResult<StudentFeeResponse>>(results.Data);
                return Result<PagedResult<StudentFeeResponse>>.Success(resultsDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
