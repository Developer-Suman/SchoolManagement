using AutoMapper;
using ES.Certificate.Application.ServiceInterface;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Certificate.Application.Certificates.Command.AddCertificateTemplate
{
    public class AddCertificateTemplateCommandHandler : IRequestHandler<AddCertificateTemplateCommand, Result<AddCertificateTemplateResponse>>
    {
        private readonly IValidator<AddCertificateTemplateCommand> _validator;
        private readonly IMapper _mapper;
        private readonly ICertificateTemplateServices _certificateTemplateServices;

        public AddCertificateTemplateCommandHandler(IValidator<AddCertificateTemplateCommand> validator, IMapper mapper, ICertificateTemplateServices certificateTemplateServices)
        {
            _certificateTemplateServices = certificateTemplateServices;
            _validator = validator;
            _mapper = mapper;


        }
        public async Task<Result<AddCertificateTemplateResponse>> Handle(AddCertificateTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddCertificateTemplateResponse>.Failure(errors);
                }

                var addCertificateTemplate = await _certificateTemplateServices.Add(request);

                if (addCertificateTemplate.Errors.Any())
                {
                    var errors = string.Join(", ", addCertificateTemplate.Errors);
                    return Result<AddCertificateTemplateResponse>.Failure(errors);
                }

                if (addCertificateTemplate is null || !addCertificateTemplate.IsSuccess)
                {
                    return Result<AddCertificateTemplateResponse>.Failure(" ");
                }

                var addCertificateTemplateDisplay = _mapper.Map<AddCertificateTemplateResponse>(addCertificateTemplate.Data);
                return Result<AddCertificateTemplateResponse>.Success(addCertificateTemplateDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Certificate template", ex);


            }
        }
    }
}
