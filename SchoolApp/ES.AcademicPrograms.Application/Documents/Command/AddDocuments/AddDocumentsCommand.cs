using MediatR;
using Microsoft.AspNetCore.Http;
using TN.Shared.Domain.Abstractions;

namespace ES.AcademicPrograms.Application.Documents.Command.AddDocuments;

public record AddDocumentsCommand(
    string applicantId,
    List<DocumentsDTOs> documentsDTOs
) : IRequest<Result<AddDocumentsResponse>>;
