using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Queries.VisaStatus.VisaStatus
{
    public class VisaStatusQueryHandler : IRequestHandler<VisaStatusQuery, Result<VisaStatusQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public VisaStatusQueryHandler(IVisaServices visaServices, IMapper mapper)
        {
            _mapper = mapper;
            _visaServices = visaServices;

        }
        public async Task<Result<VisaStatusQueryResponse>> Handle(VisaStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var query = await _visaServices.GetVisaStatus(request.id);
                return Result<VisaStatusQueryResponse>.Success(query.Data);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
