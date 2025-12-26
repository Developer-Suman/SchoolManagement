using AutoMapper;
using ES.Finances.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Queries.Fee.StudentFeeById
{
    public class StudentFeeByIdQueryHandler : IRequestHandler<StudentFeeByIdQuery, Result<StudentFeeByIdResponse>>
    {
        private readonly IStudentFeeServices _studentfeeServices;
        private readonly IMapper _mapper;

        public StudentFeeByIdQueryHandler(IStudentFeeServices studentfeeServices, IMapper mapper)
        {
            _studentfeeServices = studentfeeServices;
            _mapper = mapper;
        }
        public async Task<Result<StudentFeeByIdResponse>> Handle(StudentFeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var studentfeeById = await _studentfeeServices.GetStudentFee(request.id);
                return Result<StudentFeeByIdResponse>.Success(studentfeeById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Notice by using id", ex);
            }
        }
    }
}
