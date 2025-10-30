using AutoMapper;
using MediatR;
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusBySchool;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Authentication.Application.Authentication.Queries.GetItemStatusBySchool
{
    public class GetItemStatusByCompanyQueryHandler : IRequestHandler<GetItemStatusBySchoolQuery, Result<GetItemStatusBySchoolResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetItemStatusByCompanyQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _mapper = mapper;
            _settingServices = settingServices;

        }
        public async Task<Result<GetItemStatusBySchoolResponse>> Handle(GetItemStatusBySchoolQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var expiredDateStatus = await _settingServices.GetItemStatusBySchool(request.schoolId, cancellationToken);
                var expiredDateStatusDisplay = _mapper.Map<GetItemStatusBySchoolResponse>(expiredDateStatus.Data);
                return Result<GetItemStatusBySchoolResponse>.Success(expiredDateStatusDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting ExpiredDate Status by school{request.schoolId}");
            }
        }
    }
}
