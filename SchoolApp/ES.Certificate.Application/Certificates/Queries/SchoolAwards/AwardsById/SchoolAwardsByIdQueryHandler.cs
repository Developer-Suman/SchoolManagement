using AutoMapper;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Queries.SchoolAwards.AwardsById
{
    public class SchoolAwardsByIdQueryHandler : IRequestHandler<SchoolAwardsByIdQuery, Result<SchoolAwardsByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolAwardsServices _schoolAwardsServices;
        public SchoolAwardsByIdQueryHandler(IMapper mapper, ISchoolAwardsServices schoolAwardsServices)
        {
            _schoolAwardsServices = schoolAwardsServices;
            _mapper = mapper;

        }
        public async Task<Result<SchoolAwardsByIdResponse>> Handle(SchoolAwardsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var certificateTemplateById = await _schoolAwardsServices.GetAwards(request.id);
                return Result<SchoolAwardsByIdResponse>.Success(certificateTemplateById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Awards By ID", ex);
            }
        }
    }
}
