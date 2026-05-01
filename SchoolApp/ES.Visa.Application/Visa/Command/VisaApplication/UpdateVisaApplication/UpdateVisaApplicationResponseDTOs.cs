using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Visa.Application.Visa.Command.VisaApplication.UpdateVisaApplication
{
    public record UpdateVisaApplicationResponseDTOs
    (
        string documentTypeId,
            string FilePath,
            string visaStatusId
        );
}
