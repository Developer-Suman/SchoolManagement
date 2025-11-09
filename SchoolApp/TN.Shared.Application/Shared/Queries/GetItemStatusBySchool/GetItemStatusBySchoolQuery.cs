using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Queries.GetExpiredDateItemStatusBySchool;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.GetItemStatusBySchool
{
    public record GetItemStatusBySchoolQuery(string schoolId) : IRequest<Result<GetItemStatusBySchoolResponse>>;

}
