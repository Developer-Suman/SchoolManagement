using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.SchoolAssets.UpdateContributors
{
    public record UpdateContributorsCommand
    (
         string id,
         string name,
         string? organization,
         string? contactNumber,
         string? email
         //string schoolId
     ):IRequest<Result<UpdateContributorsResponse>>;
}
