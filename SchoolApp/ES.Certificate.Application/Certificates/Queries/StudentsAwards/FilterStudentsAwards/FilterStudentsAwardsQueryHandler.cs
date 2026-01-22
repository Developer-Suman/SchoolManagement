using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using ZXing;

namespace ES.Certificate.Application.Certificates.Queries.StudentsAwards.FilterStudentsAwards
{
    public class FilterStudentsAwardsQueryHandler : IRequestHandler<FilterStudentsAwardsQuery, Result<PagedResult<FilterStudentsAwardsResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentsAwardsServices _studentsAwardsServices;

        public FilterStudentsAwardsQueryHandler(IMapper mapper, IStudentsAwardsServices studentsAwardsServices)
        {
            _studentsAwardsServices = studentsAwardsServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<FilterStudentsAwardsResponse>>> Handle(FilterStudentsAwardsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _studentsAwardsServices.GetFilterStudentsAwards(request.PaginationRequest, request.FilterStudentsAwardsDTOs);

                var studentsAwardsResult = _mapper.Map<PagedResult<FilterStudentsAwardsResponse>>(result.Data);

                return Result<PagedResult<FilterStudentsAwardsResponse>>.Success(studentsAwardsResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterStudentsAwardsResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
