using ES.Academics.Application.Academics.Queries.MarkSheetByStudent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.ServiceInterface.IHelperMethod
{
    public interface IHelperMethodServices
    {
        Task<string> CalculatePercentage(MarksSheetDTOs marksSheetDTOs);
        Task<string> CalculateGPA(MarksSheetDTOs marksSheetDTOs);

        Task<string> CalculateDivision(MarksSheetDTOs marksSheetDTOs);
    }
}
