using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Hungry.Services.Infrastructure.Upload
{
    public class CloudinaryService : IImageUploadService
    {
        private Cloudinary clodinaryClient;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.clodinaryClient = cloudinary;
        }

        public async Task<string> UploadAsync(string localImagePath)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(localImagePath)
            };

            var uploadResult = await clodinaryClient.UploadAsync(uploadParams);
            return uploadResult.Uri.AbsoluteUri;
        }
    }
}