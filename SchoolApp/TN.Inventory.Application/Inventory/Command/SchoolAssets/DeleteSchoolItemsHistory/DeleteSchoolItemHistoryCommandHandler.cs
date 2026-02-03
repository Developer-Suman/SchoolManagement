using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteSchoolItemHistory
{
    public class DeleteSchoolItemHistoryCommandHandler : IRequestHandler<DeleteSchoolItemHistoryCommand, Result<bool>>
    {
        ISchoolAssetsServices _schoolAssetsServices;
        IMapper _mapper;
        public DeleteSchoolItemHistoryCommandHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteSchoolItemHistoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSchoolItemHistory = await _schoolAssetsServices.DeleteSchoolItemHistory(request.id, cancellationToken);
                if (deleteSchoolItemHistory is null)
                {
                    return Result<bool>.Failure("NotFound", "SchoolItemHistory are not Found");
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
