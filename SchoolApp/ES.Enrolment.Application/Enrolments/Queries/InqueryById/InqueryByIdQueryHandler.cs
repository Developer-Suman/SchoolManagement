using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Enrolment.Application.Enrolments.Queries.InqueryById
{
    public class InqueryByIdQueryHandler : IRequestHandler<InqueryByIdQuery, Result<InqueryByIdResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IEnrolmentServices _enrolmentServices;

        public InqueryByIdQueryHandler(IMapper mapper, IEnrolmentServices enrolmentServices)
        {
            _mapper = mapper;
            _enrolmentServices = enrolmentServices;
            
        }
        public async Task<Result<InqueryByIdResponse>> Handle(InqueryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var inquiry = await _enrolmentServices.GetInquiry(request.id);
                return Result<InqueryByIdResponse>.Success(inquiry.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching inquiry using id", ex);
            }
        }
    }
}
