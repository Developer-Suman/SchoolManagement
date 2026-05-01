using AutoMapper;
using ES.Visa.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Visa.Application.Visa.Queries.VisaApplication.VisaApplication
{
    public class VisaApplicationQueryhandler : IRequestHandler<VisaApplicationQuery, Result<VisaApplicationResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IVisaServices _visaServices;

        public VisaApplicationQueryhandler(IVisaServices visaServices, IMapper mapper)
        {
            _mapper = mapper;
            _visaServices = visaServices;
            
        }
        public async Task<Result<VisaApplicationResponse>> Handle(VisaApplicationQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var examResultById = await _visaServices.GetVisaApplication(request.id);
                return Result<VisaApplicationResponse>.Success(examResultById.Data);

            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
