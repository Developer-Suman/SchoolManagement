using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.DeleteExamResult
{
    public class DeleteExamResultCommandHandler : IRequestHandler<DeleteExamResultCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IExamResultServices _examResultServices;

        public DeleteExamResultCommandHandler(IMapper mapper, IExamResultServices examResultServices)
        {
            _mapper = mapper;
            _examResultServices = examResultServices;
        }
        public async Task<Result<bool>> Handle(DeleteExamResultCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteExamResult = await _examResultServices.Delete(request.id, cancellationToken);
                if (deleteExamResult is null)
                {
                    return Result<bool>.Failure("NotFound", "Exam Result not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting ExamResult", ex);
            }
        }
    }
}
