using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Application.Shared.Queries.GetCurrentFiscalYear;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetCurrentFiscalYearBySchool{
    public record  GetCurrentFiscalYearByQuery
   (string schoolId):IRequest<Result<GetCurrentFiscalYearQueryResponse>>;
}
