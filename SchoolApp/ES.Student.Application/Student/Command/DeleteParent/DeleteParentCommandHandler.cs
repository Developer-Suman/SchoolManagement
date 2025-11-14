using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ES.Student.Application.ServiceInterface;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.DeleteParent
{
    public class DeleteParentCommandHandler:IRequestHandler<DeleteParentCommand,Result<bool>>
    {
        private readonly IStudentServices _studentServices;
        private readonly IMapper _mapper;

        public DeleteParentCommandHandler(IStudentServices studentServices,IMapper mapper)
        {
            _studentServices = studentServices;
            _mapper = mapper;
            
        }

        public async Task<Result<bool>> Handle(DeleteParentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteParent = await _studentServices.DeleteParent(request.id, cancellationToken);
                if (deleteParent is null)
                {
                    return Result<bool>.Failure("NotFound", "parent not Found");
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
