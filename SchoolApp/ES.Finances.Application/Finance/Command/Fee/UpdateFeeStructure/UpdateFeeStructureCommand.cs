using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure
{
    public record UpdateFeeStructureCommand
    (
        string id,
        string classId,
            string feeCategoryId,
            List<AddFeeStructureDTOs?> feeStructureDTOs
        ) : IRequest<Result<UpdateFeeStructureResponse>>;
}
