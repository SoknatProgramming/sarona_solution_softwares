using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Sarona_Solution_Softwares.Model.Domain;
using Sarona_Solution_Softwares.Model.DomainDTOs;
using Sarona_Solution_Softwares.Repositories.Images;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sarona_Solution_Softwares.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        // GET: /<controller>/
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                // Uload Image

                var modelDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription
                };


                //use repository to upload image

                await imageRepository.Upload(modelDomainModel);

                return Ok(modelDomainModel);
            }
            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported File Extension");
            }
            if(request.File.Length >= 10485760)
            {
                ModelState.AddModelError("file", "File size is more than 10MB , Please upload a smaller size file");
            }
        }
    }
}

