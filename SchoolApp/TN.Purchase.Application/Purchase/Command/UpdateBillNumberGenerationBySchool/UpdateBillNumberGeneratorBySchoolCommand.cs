
using MediatR;
using TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.School;


namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationBySchool
{
    public record UpdateBillNumberGeneratorBySchoolCommand
    (
        BillNumberGenerationType BillNumberGenerationType,
        string schoolId
        ) : IRequest<Result<UpdateBillNumberGeneratorBySchoolResponse>>;
}
