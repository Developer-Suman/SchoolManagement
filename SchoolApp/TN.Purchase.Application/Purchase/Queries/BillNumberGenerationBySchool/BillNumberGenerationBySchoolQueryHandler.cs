using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.Purchase.Queries.BillNumberGenerationBySchool
{
    public class BillNumberGenerationBySchoolQueryHandler : IRequestHandler<BillNumberGenerationBySchoolQueries, Result<BillNumberGenerationBySchoolQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;

        public BillNumberGenerationBySchoolQueryHandler(IMapper mapper, IPurchaseDetailsServices purchaseDetailsServices)
        {
            _mapper = mapper;
            _purchaseDetailsServices = purchaseDetailsServices;

        }
        public async Task<Result<BillNumberGenerationBySchoolQueryResponse>> Handle(BillNumberGenerationBySchoolQueries request, CancellationToken cancellationToken)
        {
            try
            {
                var allPurchaseDetails = await _purchaseDetailsServices.GetBillNumberStatusByCompany(request.id, cancellationToken);

                if (allPurchaseDetails == null)
                {
                    return Result<BillNumberGenerationBySchoolQueryResponse>.Failure("Purchase details not found.");
                }

                return Result<BillNumberGenerationBySchoolQueryResponse>.Success(allPurchaseDetails.Data);


            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all purchaseDetails ", ex);
            }
        }
    }
}
