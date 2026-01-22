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

namespace ES.Certificate.Application.Certificates.Queries.StudentsAwards.AwardsById
{
    public class AwardsByIdQueryHandler : IRequestHandler<AwardsByIdQuery, Result<AwardsByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentsAwardsServices _awardsServices;
        public AwardsByIdQueryHandler(IMapper mapper, IStudentsAwardsServices awardsServices)
        {
            _awardsServices = awardsServices;
            _mapper = mapper;

        }
        public async Task<Result<AwardsByIdResponse>> Handle(AwardsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var certificateTemplateById = await _awardsServices.GetAwards(request.id);
                return Result<AwardsByIdResponse>.Success(certificateTemplateById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Awards By ID", ex);
            }
        }
    }
}
