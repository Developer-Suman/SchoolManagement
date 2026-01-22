using AutoMapper;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.DeleteAwards
{
    public class DeleteAwardsCommandHandler : IRequestHandler<DeleteAwardsCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IStudentsAwardsServices _awardsServices;
        public DeleteAwardsCommandHandler(IMapper mapper, IStudentsAwardsServices awardsServices)
        {
            _awardsServices = awardsServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteAwardsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteAwards= await _awardsServices.Delete(request.id, cancellationToken);
                if (_awardsServices is null)
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
