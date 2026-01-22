using AutoMapper;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.DeleteAwards
{
    public class DeleteAwardsCommandHandler : IRequestHandler<DeleteSchoolAwardsCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ISchoolAwardsServices _schoolAwardsServices;
        public DeleteAwardsCommandHandler(IMapper mapper, ISchoolAwardsServices schoolAwardsServices)
        {
            _schoolAwardsServices = schoolAwardsServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteSchoolAwardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteAwards= await _schoolAwardsServices.Delete(request.id, cancellationToken);
                if (_schoolAwardsServices is null)
                {
                    return Result<bool>.Failure("NotFound", "Awards not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting Awards", ex);
            }
        }
    }
}
