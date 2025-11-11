using AutoMapper;
using ES.Certificate.Application.Certificates.Command.DeleteIssuedCertificate;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.DeleteCertificateTemplate
{
    public class DeleteCertificateTemplateCommandHandler : IRequestHandler<DeleteCertificateTemplateCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly ICertificateTemplateServices _certificateTemplateServices;

        public DeleteCertificateTemplateCommandHandler(IMapper mapper, ICertificateTemplateServices certificateTemplateServices)
        {
            _certificateTemplateServices = certificateTemplateServices;
            _mapper = mapper;
            
        }

        public async Task<Result<bool>> Handle(DeleteCertificateTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteCertificateTemplate = await _certificateTemplateServices.Delete(request.id, cancellationToken);
                if (deleteCertificateTemplate is null)
                {
                    return Result<bool>.Failure("NotFound", "Certificate Template not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting Certificate Template", ex);
            }
        }
    }
}
