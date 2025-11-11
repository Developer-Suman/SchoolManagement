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

namespace ES.Certificate.Application.Certificates.Command.UpdateCertificateTemplate
{
    public class UpdateCertificateTemplateCommandHandler : IRequestHandler<UpdateCertificateTemplateCommand, Result<UpdateCertificateTemplateResponse>>
    {
        private readonly IValidator<UpdateCertificateTemplateCommand> _validator;
        public readonly IMapper _mapper;
        private readonly ICertificateTemplateServices _certificateTemplateServices;

        public UpdateCertificateTemplateCommandHandler(IValidator<UpdateCertificateTemplateCommand> validator, ICertificateTemplateServices certificateTemplateServices,IMapper mapper)
        {
            _certificateTemplateServices = certificateTemplateServices;
            _validator = validator;
            _mapper = mapper;
            
        }

        public async Task<Result<UpdateCertificateTemplateResponse>> Handle(UpdateCertificateTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateCertificateTemplateResponse>.Failure(errors);

                }

                var updateCertificateTemplate = await _certificateTemplateServices.Update(request.Id, request);

                if (updateCertificateTemplate.Errors.Any())
                {
                    var errors = string.Join(", ", updateCertificateTemplate.Errors);
                    return Result<UpdateCertificateTemplateResponse>.Failure(errors);
                }

                if (updateCertificateTemplate is null || !updateCertificateTemplate.IsSuccess)
                {
                    return Result<UpdateCertificateTemplateResponse>.Failure("Updates Exam failed");
                }

                var UpdateCertificateTemplateDisplay = _mapper.Map<UpdateCertificateTemplateResponse>(updateCertificateTemplate.Data);
                return Result<UpdateCertificateTemplateResponse>.Success(UpdateCertificateTemplateDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Certificate Template", ex);
            }
        }
    }
}
