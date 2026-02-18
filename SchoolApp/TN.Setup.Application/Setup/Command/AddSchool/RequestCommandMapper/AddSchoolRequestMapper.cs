using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using TN.Setup.Application.Setup.Command.AddInstitution;
using TN.Setup.Application.Setup.Command.AddSchool;
using TN.Setup.Domain.Entities;

namespace TN.Setup.Application.Setup.Command.AddSchool.RequestCommandMapper
{
    public static class AddSchoolRequestMapper
    {
        public static AddSchoolCommand ToCommand(this AddSchoolRequest addSchoolRequest)
        {
          return new AddSchoolCommand
                     (

                       addSchoolRequest.name,
                        addSchoolRequest.address,
                        addSchoolRequest.shortName,
                       addSchoolRequest.email,
                        addSchoolRequest.contactNumber,
                       addSchoolRequest.contactPerson,
                       addSchoolRequest.pan,
                       addSchoolRequest.imageUrl,
                        addSchoolRequest.isEnabled,
                       addSchoolRequest.institutionId,
                        addSchoolRequest.isDeleted,
                        addSchoolRequest.fiscalYearId,
                        addSchoolRequest.academicYearId,
                        addSchoolRequest.billNumberGenerationTypeForPurchase,
                        addSchoolRequest.billNumberGenerationTypeForSales
                        
                        

                      );


        }
    }
}
