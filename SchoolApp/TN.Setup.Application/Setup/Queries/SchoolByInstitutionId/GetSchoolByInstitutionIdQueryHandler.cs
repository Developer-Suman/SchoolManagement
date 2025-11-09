using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Queries.InstitutionByOrganizationId;
using TN.Setup.Application.Setup.Queries.SchoolByInstitutionId;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.CompanyByInstitutionId
{
    public sealed class GetSchoolByInstitutionIdQueryHandler : IRequestHandler<GetSchoolByInstitutionIdQuery, Result<List<GetSchoolByInstitutionIdResponse>>>
    {
        private readonly ISchoolServices _schoolServices;
        private readonly IMapper _mapper;


        public GetSchoolByInstitutionIdQueryHandler(ISchoolServices schoolServices,IMapper mapper)
        {
            _schoolServices = schoolServices;
            _mapper=mapper;
            
        }

        public async Task<Result<List<GetSchoolByInstitutionIdResponse>>> Handle(GetSchoolByInstitutionIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var schoolByInstitutionId = await _schoolServices.GetSchoolByInstitutionId(request.institutionId, cancellationToken);
                return Result<List<GetSchoolByInstitutionIdResponse>>.Success(schoolByInstitutionId.Data);


            }
            catch (Exception ex) 
            {
              throw new Exception("An error occurred while fetching a school by institution id",ex);
            
            }
        }
    }
}
