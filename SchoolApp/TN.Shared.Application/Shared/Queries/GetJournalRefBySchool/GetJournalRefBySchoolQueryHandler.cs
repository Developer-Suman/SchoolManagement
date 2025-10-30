using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Queries.GetPaymentTransactionNumberType;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetJournalRefBySchool
{
    public  class GetJournalRefBySchoolQueryHandler:IRequestHandler<GetJournalRefBySchoolQuery, Result<GetJournalRefBySchoolQueryResponse>>
    
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;

        public GetJournalRefBySchoolQueryHandler(ISettingServices settingServices, IMapper mapper)
        {
            _settingServices=settingServices;
            _mapper=mapper;
        }

        public async Task<Result<GetJournalRefBySchoolQueryResponse>> Handle(GetJournalRefBySchoolQuery request, CancellationToken cancellationToken)
        {

            var journalRef = await _settingServices.GetJournalRefBySchool(request.schoolId, cancellationToken);

            if (journalRef is not { IsSuccess: true, Data: not null })
                return Result<GetJournalRefBySchoolQueryResponse>.Failure(journalRef?.Message ?? $"Getting Journal Reference Number by school {request.schoolId}");

            return Result<GetJournalRefBySchoolQueryResponse>.Success(
                _mapper.Map<GetJournalRefBySchoolQueryResponse>(journalRef.Data)
            );
        }
    }
}
