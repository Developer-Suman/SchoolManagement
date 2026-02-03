using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.DeleteContributors
{
    public class DeleteContributorsCommandHandler : IRequestHandler<DeleteContributorsCommand, Result<bool>>
    {
        ISchoolAssetsServices _schoolAssetsServices;
        IMapper _mapper;
        public DeleteContributorsCommandHandler(ISchoolAssetsServices schoolAssetsServices, IMapper mapper)
        {
            _schoolAssetsServices = schoolAssetsServices;
            _mapper = mapper;
        }
        public async Task<Result<bool>> Handle(DeleteContributorsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteContributors = await _schoolAssetsServices.DeleteContributors(request.id, cancellationToken);
                if (deleteContributors is null)
                {
                    return Result<bool>.Failure("NotFound", "Contributors are not Found");
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
