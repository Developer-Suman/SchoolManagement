using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetInventoryMethodBySchool
{
    public  class GetInventoryBySchoolIdQueryHandler:IRequestHandler<GetInventoryBySchoolIdQuery, Result<GetInventoryBySchoolIdQueryResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetInventoryBySchoolIdQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
        }

        public async Task<Result<GetInventoryBySchoolIdQueryResponse>> Handle(GetInventoryBySchoolIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var inventoryMethod = await _settingServices.GetInventoryMethodBySchool(request.schoolId, cancellationToken);
                var inventoryMethodDisplay = _mapper.Map<GetInventoryBySchoolIdQueryResponse>(inventoryMethod.Data);

                return Result<GetInventoryBySchoolIdQueryResponse>.Success(inventoryMethodDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while Getting inventory method by school {request.schoolId}");
            }
        }
    }
}
