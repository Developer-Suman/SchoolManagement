
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.School;


namespace TN.Setup.Application.Setup.Command.AddSchool
{
    public record AddSchoolCommand
    (
            string name,
            string address,
            string shortName,
            string email,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            bool isEnabled,
            string institutionId,
            bool isDeleted,
            string? fiscalYearId,
            string? academicYearId,
            BillNumberGenerationType billNumberGenerationTypeForPurchase,
            BillNumberGenerationType billNumberGenerationTypeForSales
    ) : IRequest<Result<AddSchoolResponse>>;
}
