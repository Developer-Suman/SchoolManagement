using AutoMapper;
using ES.Academics.Application.Academics.Queries.FilterExam;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.FilterSubject
{
    public class FilterSubjectQueryHandler : IRequestHandler<FilterSubjectQuery, Result<PagedResult<FilterSubjectResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ISubjectServices _subjectServices;

        public FilterSubjectQueryHandler(IMapper mapper, ISubjectServices subjectServices)
        {
            _mapper = mapper;
            _subjectServices = subjectServices;
        }
        public async Task<Result<PagedResult<FilterSubjectResponse>>> Handle(FilterSubjectQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _subjectServices.GetFilterSubject(request.PaginationRequest, request.FilterSubjectDTOs);

                var subjectResult = _mapper.Map<PagedResult<FilterSubjectResponse>>(result.Data);

                return Result<PagedResult<FilterSubjectResponse>>.Success(subjectResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSubjectResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
