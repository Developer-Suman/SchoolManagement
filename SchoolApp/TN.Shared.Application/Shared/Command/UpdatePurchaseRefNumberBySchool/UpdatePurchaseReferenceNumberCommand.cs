using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.SchoolSettings;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberBySchool
{
    public record  UpdatePurchaseReferenceNumberCommand
    (

             PurchaseReferencesType PurchaseReferencesType,
            string schoolId
    ) :IRequest<Result<UpdatePurchaseReferenceNumberResponse>>;
}
