using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItemsHistoryById;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolItemsById
{
    public class SchoolItemsByIdQueryHandler : IRequestHandler<SchoolItemsByIdQuery, Result<SchoolItemsByIdResponse>>
    {

        private readonly IMapper _mapper;
        private readonly ISchoolAssetsServices _schoolAssetsServices;

        public SchoolItemsByIdQueryHandler(IMapper mapper, ISchoolAssetsServices schoolAssetsServices)
        {
            _mapper = mapper;
            _schoolAssetsServices = schoolAssetsServices;

        }
        public async Task<Result<SchoolItemsByIdResponse>> Handle(SchoolItemsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _schoolAssetsServices.GetSchoolItemById(request.id);
                return Result<SchoolItemsByIdResponse>.Success(result.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching", ex);
            }
        }
    }
}
