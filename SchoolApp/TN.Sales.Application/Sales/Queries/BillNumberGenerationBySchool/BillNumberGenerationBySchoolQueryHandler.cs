using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.BillNumberGenerationBySchool
{
 public class BillNumberGenerationBySchoolQueryHandler:IRequestHandler<BIllNumberGenerationBySchoolQuery, Result<BIllNumberGenerationBySchoolQueryResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public BillNumberGenerationBySchoolQueryHandler(ISalesDetailsServices salesDetailsServices,IMapper mapper)
        {
            _salesDetailsServices = salesDetailsServices;
                _mapper=mapper;
        }

        public async  Task<Result<BIllNumberGenerationBySchoolQueryResponse>> Handle(BIllNumberGenerationBySchoolQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSalesDetails = await _salesDetailsServices.GetBillNumberStatusBySchool(request.id, cancellationToken);

                if (allSalesDetails == null)
                {
                    return Result<BIllNumberGenerationBySchoolQueryResponse>.Failure("sales details not found.");
                }

                return Result<BIllNumberGenerationBySchoolQueryResponse>.Success(allSalesDetails.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching billNumber status by company ", ex);
            }
        }
    }
}
