using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.SchoolAssetsReport
{
    public record SchoolAssetsReportResponse
    (
        string contributorName,
        string fiscalYearName,
        decimal? totalEstimatedValue,
        decimal? totalItemsCount,
        string itemsName


        );
    
}
