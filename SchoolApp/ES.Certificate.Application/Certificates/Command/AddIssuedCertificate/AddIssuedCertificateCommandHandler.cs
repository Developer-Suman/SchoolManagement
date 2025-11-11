using AutoMapper;
using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.AddIssuedCertificate
{
    public class AddIssuedCertificateCommandHandler : IRequestHandler<AddIssuedCertificateCommand, Result<AddIssuedCertificateResponse>>
    {

        private readonly IMapper _mapper;
        private readonly IValidator<AddIssuedCertificateCommand> _validator;
        private readonly IIssuedCertificateServices _issuedCertificateServices;

        public AddIssuedCertificateCommandHandler(IMapper mapper, IValidator<AddIssuedCertificateCommand> validator, IIssuedCertificateServices issuedCertificateServices)
        {
           _issuedCertificateServices = issuedCertificateServices;
            _mapper = mapper;
            _validator = validator;
            
        }
        public async Task<Result<AddIssuedCertificateResponse>> Handle(AddIssuedCertificateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddIssuedCertificateResponse>.Failure(errors);
                }

                var addIssuedCertificate = await _issuedCertificateServices.Add(request);

                if (addIssuedCertificate.Errors.Any())
                {
                    var errors = string.Join(", ", addIssuedCertificate.Errors);
                    return Result<AddIssuedCertificateResponse>.Failure(errors);
                }

                if (addIssuedCertificate is null || !addIssuedCertificate.IsSuccess)
                {
                    return Result<AddIssuedCertificateResponse>.Failure(" ");
                }

                var addIssuedCertificateDisplay = _mapper.Map<AddIssuedCertificateResponse>(addIssuedCertificate.Data);
                return Result<AddIssuedCertificateResponse>.Success(addIssuedCertificateDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding IssuedCertificate", ex);


            }
        }
    }
}
