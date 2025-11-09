using AutoMapper;
using ES.Academics.Application.Academics.Command.AddSchoolClass;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Academics.Application.Academics.Queries.SchoolClass
{
    public sealed class SchoolClassQueryHandler : IRequestHandler<SchoolClassQuery, Result<PagedResult<SchoolClassQueryResponse>>>
    {
        private readonly ISchoolClassInterface _schoolClassInterface;
        private readonly IMapper _mapper;

        public SchoolClassQueryHandler(ISchoolClassInterface schoolClassInterface, IMapper mapper)
        {
            _schoolClassInterface = schoolClassInterface;
            _mapper = mapper;

        }

        public async Task<Result<PagedResult<SchoolClassQueryResponse>>> Handle(SchoolClassQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var schoolClass = await _schoolClassInterface.GetSchoolClass(request.PaginationRequest);
                var schoolClassResult = _mapper.Map<PagedResult<SchoolClassQueryResponse>>(schoolClass.Data);
                return Result<PagedResult<SchoolClassQueryResponse>>.Success(schoolClassResult);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching all customer", ex);
            }
        }
    }
}
