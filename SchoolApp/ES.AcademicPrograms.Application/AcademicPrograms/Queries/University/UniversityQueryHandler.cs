using AutoMapper;
using ES.AcademicPrograms.Application.AcademicPrograms.Queries.Course;
using ES.AcademicPrograms.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Queries.University
{
    public class UniversityQueryHandler : IRequestHandler<UniversityQuery, Result<PagedResult<UniversityResponse>>>
    {

        private readonly IUniversityServices _universityServices;
        private readonly IMapper _mapper;

        public UniversityQueryHandler(IUniversityServices universityServices, IMapper mapper)
        {
            _universityServices = universityServices;
            _mapper = mapper;

        }


        public async Task<Result<PagedResult<UniversityResponse>>> Handle(UniversityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var all = await _universityServices.GetAllUniversity(request.PaginationRequest);
                var allDetails = _mapper.Map<PagedResult<UniversityResponse>>(all.Data);
                return Result<PagedResult<UniversityResponse>>.Success(allDetails);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all students", ex);
            }
        }
    }
}
