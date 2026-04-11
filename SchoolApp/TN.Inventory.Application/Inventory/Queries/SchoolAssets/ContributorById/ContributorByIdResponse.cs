using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.SchoolAssets.ContributorById
{
    public record ContributorByIdResponse
    (
        string id="",
            string name="",
            string? organization="",
            string? contactNumber = "",
            string? email = "",
            string schoolId = "",
            bool isActive=true,
            string createdBy = "",
            DateTime createdAt=default,
            string modifiedBy = "",

            DateTime modifiedAt=default
        );
}
