using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.CocurricularActivities.Command.Activity.DeleteActivity
{
    public record DeleteActivityCommand
    (
        string id
        ) : IRequest<Result<bool>>;
}
