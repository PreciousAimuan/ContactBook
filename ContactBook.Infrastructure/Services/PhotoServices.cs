using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ContactBook.Infrastructure.Helper;
using ContactBook.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Infrastructure.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account  // to access
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult(); //method for uploading images and cloudinary return this
            if (file.Length > 0) //Checks if there is at least 1 file
            {
                using var stream = file.OpenReadStream(); //Reads the file

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream), //Grabs the name of the file that is uploaded
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face") //transforms the image
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);  //uploads the file to cloudinary
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            throw new NotImplementedException();
        }
    }
}
