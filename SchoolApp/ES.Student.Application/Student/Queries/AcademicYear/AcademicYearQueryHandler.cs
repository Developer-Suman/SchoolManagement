using AutoMapper;
using ES.Student.Application.ServiceInterface;
using ES.Student.Application.Student.Queries.GetAllStudents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Student.Application.Student.Queries.AcademicYear
{
    public class AcademicYearQueryHandler : IRequestHandler<AcademicYearQuery, Result<PagedResult<AcademicYearResponse>>>
    {
        private readonly IStudentServices _studnetServices;
        private readonly IMapper _mapper;

        public AcademicYearQueryHandler(IStudentServices studentServices, IMapper mapper)
        {
            _studnetServices = studentServices;
            _mapper = mapper;
        }
        public async Task<Result<PagedResult<AcademicYearResponse>>> Handle(AcademicYearQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var academicYear = await _studnetServices.GetAllAcademicYear(request.PaginationRequest, cancellationToken);
                var academicYearDisplay = _mapper.Map<PagedResult<AcademicYearResponse>>(academicYear.Data);
                return Result<PagedResult<AcademicYearResponse>>.Success(academicYearDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
