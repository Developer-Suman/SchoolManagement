using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Certificate.Application.ServiceInterface.IHelperMethod
{
    public interface IHelperMethodServices
    {
        Task<string> CalculatePercentage(string studentId);
        Task<string> CalculateGPA(string studentId);
    }
}
