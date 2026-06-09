using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.Documents.Queries.DocumentsType.FilterDocumentsType
{
    public class FilterDocumentsTypeQueryhandler : IRequestHandler<FilterDocumentsTypeQuery, Result<PagedResult<FilterDocumentsTypeResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IDocumentsServices _documentsServices;

        public FilterDocumentsTypeQueryhandler(IMapper mapper, IDocumentsServices documentsServices)
        {
            _mapper = mapper;
            _documentsServices = documentsServices;
        }
        public async Task<Result<PagedResult<FilterDocumentsTypeResponse>>> Handle(FilterDocumentsTypeQuery request, CancellationToken cancellationToken)
        {
            var entityName = typeof(FilterDocumentsTypeQuery).Name
             .Replace("Filter", "").Replace("Query", "");
            try
            {

                var result = await _documentsServices.FilterDocumentsType(request.FilterDocumentsTypeDTOs, request.PaginationRequest);

                var filterResult = _mapper.Map<PagedResult<FilterDocumentsTypeResponse>>(result.Data);

                return Result<PagedResult<FilterDocumentsTypeResponse>>.Success(filterResult, $"{entityName} return successfully");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
