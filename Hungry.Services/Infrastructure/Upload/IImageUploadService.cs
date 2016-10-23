using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hungry.Services.Infrastructure.Upload
{
    public interface IImageUploadService
    {
        Task<string> UploadAsync(string localImagePath);
    }
}