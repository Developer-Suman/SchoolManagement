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

namespace ES.Enrolment.Application.Enrolments.Queries.Counselor
{
    public class CounselorQueryHandler : IRequestHandler<CounselorQuery, Result<PagedResult<CounselorResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly ICounselorServices _counselorServices;

        public CounselorQueryHandler(IMapper mapper, ICounselorServices counselorServices)
        {
            _mapper = mapper;
            _counselorServices = counselorServices;

        }
        public async Task<Result<PagedResult<CounselorResponse>>> Handle(CounselorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _counselorServices.AllCounselor(request.PaginationRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching", ex);
            }
        }
    }
}
