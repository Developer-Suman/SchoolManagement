﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetInventoryMethodBySchool
{
    public record  GetInventoryBySchoolIdQuery
   (string schoolId) :IRequest<Result<GetInventoryBySchoolIdQueryResponse>>;
}
