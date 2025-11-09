
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;

using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetSchoolDetailsBySchoolId
{
    public  class GetSchoolDetailsBySchoolIdQueryHandler:IRequestHandler<GetSchoolDetailsBySchoolIdQuery, Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>>
    {
        private readonly ISchoolServices _schoolServices;
        private readonly IMapper _mapper;

        public GetSchoolDetailsBySchoolIdQueryHandler(ISchoolServices schoolServices,IMapper mapper)
        {
            _schoolServices = schoolServices;
            _mapper=mapper;
        }

        public async Task<Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>> Handle(GetSchoolDetailsBySchoolIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var schoolDetails = await _schoolServices.GetSchoolDetailsByInstitutionId(request.institutionId, cancellationToken);
                return Result<List<GetSchoolDetailsBySchoolIdQueryResponse>>.Success(schoolDetails.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching school details", ex);
            }
        }
    }
}
