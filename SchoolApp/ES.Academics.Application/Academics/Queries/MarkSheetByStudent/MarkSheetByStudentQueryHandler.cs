using AutoMapper;
using ES.Academics.Application.Academics.Queries.FilterSubject;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.MarkSheetByStudent
{
    public class MarkSheetByStudentQueryHandler : IRequestHandler<MarkSheetByStudentQuery, Result<MarkSheetByStudentResponse>>
    {
        private readonly IExamResultServices _examResultServices;
        private readonly IMapper _mapper;

        public MarkSheetByStudentQueryHandler(IExamResultServices examResultServices, IMapper mapper)
        {
            _mapper = mapper;
            _examResultServices = examResultServices;

        }
        public async Task<Result<MarkSheetByStudentResponse>> Handle(MarkSheetByStudentQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var marksheet = await _examResultServices.GetMarkSheet(request.MarksSheetDTOs);

                var markSheetResult = _mapper.Map<MarkSheetByStudentResponse>(marksheet.Data);

                return Result<MarkSheetByStudentResponse>.Success(markSheetResult);
            }
            catch (Exception ex)
            {
                return Result<MarkSheetByStudentResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
