using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.DeleteExam
{
    public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, Result<bool>>
    {
        private readonly IExamServices _examServices;
        private readonly IMapper _mapper;
        public DeleteExamCommandHandler(IExamServices examServices, IMapper mapper)
        {
            _examServices = examServices;
            _mapper = mapper;
            
        }
        public async Task<Result<bool>> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteExam = await _examServices.Delete(request.id, cancellationToken);
                if (deleteExam is null)
                {
                    return Result<bool>.Failure("NotFound", "Exam not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting Exam", ex);
            }
        }
    }
}
