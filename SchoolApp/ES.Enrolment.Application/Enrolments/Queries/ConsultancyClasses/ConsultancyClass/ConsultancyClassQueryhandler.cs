using AutoMapper;
using ES.Enrolment.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Enrolment.Application.Enrolments.Queries.ConsultancyClasses.ConsultancyClass
{
    public class ConsultancyClassQueryhandler : IRequestHandler<ConsultancyClassQuery, Result<PagedResult<ConsultancyClassResponse>>>
    {

        private readonly IMapper _mapper;
        private readonly IConsultancyClassServices _consultancyClassServices;

        public ConsultancyClassQueryhandler(IMapper mapper, IConsultancyClassServices consultancyClassServices)
        {
            _mapper = mapper;
            _consultancyClassServices = consultancyClassServices;

        }


        public async Task<Result<PagedResult<ConsultancyClassResponse>>> Handle(ConsultancyClassQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _consultancyClassServices.All(request.PaginationRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
