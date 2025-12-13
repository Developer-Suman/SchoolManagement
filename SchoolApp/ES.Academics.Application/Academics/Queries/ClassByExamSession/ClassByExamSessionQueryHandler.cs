using AutoMapper;
using ES.Academics.Application.Academics.Queries.Exam;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.ClassByExamSession
{
    public class ClassByExamSessionQueryHandler : IRequestHandler<ClassByExamSessionQuery, Result<PagedResult<ClassByExamSessionResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISeatPlanningServices _seatPlanningServices;
        public ClassByExamSessionQueryHandler(IMapper mapper, ISeatPlanningServices seatPlanningServices)
        {
            _mapper = mapper;
            _seatPlanningServices = seatPlanningServices;
        }

        public async Task<Result<PagedResult<ClassByExamSessionResponse>>> Handle(ClassByExamSessionQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var exam = await _seatPlanningServices.GetClassByExamSession(request.PaginationRequest, request.examSessionId);
                var examResult = _mapper.Map<PagedResult<ClassByExamSessionResponse>>(exam.Data);
                return Result<PagedResult<ClassByExamSessionResponse>>.Success(examResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all exam", ex);
            }
        }
    }
     
}
