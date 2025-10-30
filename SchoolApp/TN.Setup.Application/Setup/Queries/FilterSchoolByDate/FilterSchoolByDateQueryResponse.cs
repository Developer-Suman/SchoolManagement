﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.FilterSchoolByDate
{
    public record FilterSchoolByDateQueryResponse
    (      
            string id,
            string name,
            string address,
            string shortName,
            string email,
            string contactNumber,
            string contactPerson,
            string pan,
            string imageUrl,
            bool isEnabled,
            string institutionId
    );
}
