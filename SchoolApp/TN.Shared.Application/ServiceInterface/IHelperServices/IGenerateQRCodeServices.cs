using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.ServiceInterface.IHelperServices
{
    public interface IGenerateQRCodeServices
    {
        Task<string> GenerateSvgAsync(string values);
    }
}
