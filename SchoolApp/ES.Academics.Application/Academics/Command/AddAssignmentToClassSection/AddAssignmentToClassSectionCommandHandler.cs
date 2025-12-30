using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Command.AddAssignmentToClassSection
{
    public class AddAssignmentToClassSectionCommandHandler : IRequestHandler<AddAssignmentToClassSectionCommand, Result<AddAssignmentToClassSectionResponse>>
    {
        public Task<Result<AddAssignmentToClassSectionResponse>> Handle(AddAssignmentToClassSectionCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
