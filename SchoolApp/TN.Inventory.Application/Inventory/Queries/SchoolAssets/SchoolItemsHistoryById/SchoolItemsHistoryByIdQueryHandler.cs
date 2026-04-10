using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.StockCentersById;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItemsHistoryById
{
    public class SchoolItemsHistoryByIdQueryHandler : IRequestHandler<SchoolItemsHistoryByIdQuery, Result<SchoolItemsHistoryByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolAssetsServices _schoolAssetsServices;

        public SchoolItemsHistoryByIdQueryHandler(IMapper mapper, ISchoolAssetsServices schoolAssetsServices)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<SchoolItemsHistoryByIdResponse>> Handle(SchoolItemsHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolAssetsServices.GetSchoolItemsHistoryById(request.id);
                return Result<SchoolItemsHistoryByIdResponse>.Success(result.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }
    }
}
