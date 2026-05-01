using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaApplication.AddVisaApplication
{
    public record VisaApplicationDocumentsResponseDTOs
    (
        string documentTypeId,
            string FilePath,
            string visaStatusId
        );
}
