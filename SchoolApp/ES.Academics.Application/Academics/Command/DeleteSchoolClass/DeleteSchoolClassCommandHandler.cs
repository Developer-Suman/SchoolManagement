using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.DeleteSchoolClass
{
    public class DeleteSchoolClassCommandHandler : IRequestHandler<DeleteSchoolClassCommand, Result<bool>>
    {
        private readonly ISchoolClassInterface _schoolClassInterface;
        public DeleteSchoolClassCommandHandler(ISchoolClassInterface schoolClassInterface)
        {
            _schoolClassInterface = schoolClassInterface;
        }
        public async Task<Result<bool>> Handle(DeleteSchoolClassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteSchoolClass = await _schoolClassInterface.Delete(request.id, cancellationToken);
                if (deleteSchoolClass is null)
                {
                    return Result<bool>.Failure("NotFound", "SchoolClass not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting School Class", ex);
            }
        }
    }

}
