using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.DeleteSubject
{
    public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ISubjectServices _subjectServices;

        public DeleteSubjectCommandHandler(IMapper mapper, ISubjectServices subjectServices)
        {
            _mapper = mapper;
            _subjectServices = subjectServices;
        }
        public async Task<Result<bool>> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSubject = await _subjectServices.Delete(request.id, cancellationToken);
                if (deleteSubject is null)
                {
                    return Result<bool>.Failure("NotFound", "Subject not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting Subject", ex);
            }
        }
    }
}
