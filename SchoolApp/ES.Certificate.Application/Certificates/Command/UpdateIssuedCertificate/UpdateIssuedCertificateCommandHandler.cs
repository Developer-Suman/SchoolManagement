using AutoMapper;
using ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate;
using ES.Certificate.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.UpdateIssuedCertificate
{
    public class UpdateIssuedCertificateCommandHandler : IRequestHandler<UpdateIssuedCertificateCommand, Result<UpdateIssuedCertificateResponse>>
    {
        private readonly IValidator<UpdateIssuedCertificateCommand> _validator;
        public readonly IMapper _mapper;
        private readonly IIssuedCertificateServices _issuedCertificateServices;


        public UpdateIssuedCertificateCommandHandler(IValidator<UpdateIssuedCertificateCommand> validator,IIssuedCertificateServices issuedCertificateServices, IMapper mapper)
        {
            _validator = validator;
            _mapper = mapper;
            _issuedCertificateServices = issuedCertificateServices;

            
        }
        public async Task<Result<UpdateIssuedCertificateResponse>> Handle(UpdateIssuedCertificateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateIssuedCertificateResponse>.Failure(errors);

                }

                var updateIssuedCertificate = await _issuedCertificateServices.Update(request.Id, request);

                if (updateIssuedCertificate.Errors.Any())
                {
                    var errors = string.Join(", ", updateIssuedCertificate.Errors);
                    return Result<UpdateIssuedCertificateResponse>.Failure(errors);
                }

                if (updateIssuedCertificate is null || !updateIssuedCertificate.IsSuccess)
                {
                    return Result<UpdateIssuedCertificateResponse>.Failure("Updates Exam failed");
                }

                var updateIssuedCertificateDisplay = _mapper.Map<UpdateIssuedCertificateResponse>(updateIssuedCertificate.Data);
                return Result<UpdateIssuedCertificateResponse>.Success(updateIssuedCertificateDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Issued Certificate", ex);
            }
        }
    }
}
