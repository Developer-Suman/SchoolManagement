using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.StockDetailReport.Queries;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IStockDetailReportService
    {
        Task<Result<PagedResult<GetStockDetailReportResponse>>> GetStockDetailReport(string schoolId,PaginationRequest paginationRequest,FilterStockDetailReportDto filterStockDetailReportDto,CancellationToken cancellationToken=default);
   
        
    
    }
}
