using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteSchoolItems
{
    public class DeleteSchoolItemsCommandHandler : IRequestHandler<DeleteSchoolItemsCommand, Result<bool>>
    {
        private readonly ISchoolAssetsServices _schoolAssetsServices;
        private readonly IMapper _mapper;
        public DeleteSchoolItemsCommandHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
        }
        public async Task<Result<bool>> Handle(DeleteSchoolItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSchoolItems = await _schoolAssetsServices.DeleteSchoolItems(request.id, cancellationToken);
                if (deleteSchoolItems is null)
                {
                    return Result<bool>.Failure("NotFound", "SchooolItems are not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
