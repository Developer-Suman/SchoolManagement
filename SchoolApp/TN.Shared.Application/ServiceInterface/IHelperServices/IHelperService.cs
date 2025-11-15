using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.ServiceInterface.IHelperServices
{
    public interface IHelperService
    {
        Task<int> CalculateAge(DateTime dateOfBirth, DateTime now);
        bool IsImage(string contentType);
        void CompressFile(string inputFilePath, string outputFilePath);
        bool CompareImage(IFormFile imagePath1, string imagePath2);
    }
}
