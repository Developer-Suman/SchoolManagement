using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.ServiceInterface.IHelperServices
{
    public interface IimageServices
    {
        Task<string> AddSingle(IFormFile File);
        Task<List<string>> AddMultiple(List<IFormFile> File);
        void DeleteSingle(string ImageUrl);
        void DeleteMultiple(List<string> ImageUrls);
        Task<string> UpdateSingle(IFormFile file, string ImageUrl);
        Task<List<string>> UpdateMultiple(List<IFormFile> file, List<string> ImageUrls);
    }
}
