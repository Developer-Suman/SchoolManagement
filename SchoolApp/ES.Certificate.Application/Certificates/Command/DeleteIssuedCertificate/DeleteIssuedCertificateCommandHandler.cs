using AutoMapper;
using ES.Certificate.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.DeleteIssuedCertificate
{
    public class DeleteIssuedCertificateCommandHandler : IRequestHandler<DeleteIssuedCertificateCommand, Result<bool>>
    {
        private readonly IMapper _mapper;
        private readonly IIssuedCertificateServices _issuedCertificateServices;

        public DeleteIssuedCertificateCommandHandler(IMapper mapper, IIssuedCertificateServices issuedCertificateServices)
        {
            _issuedCertificateServices = issuedCertificateServices;
            _mapper = mapper;

        }
        public async Task<Result<bool>> Handle(DeleteIssuedCertificateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteIssuedCertificate = await _issuedCertificateServices.Delete(request.id, cancellationToken);
                if (deleteIssuedCertificate is null)
                {
                    return Result<bool>.Failure("NotFound", "Issued Certificate not Found");
                }
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting Issued Certificate", ex);
            }
        }
    }
}
