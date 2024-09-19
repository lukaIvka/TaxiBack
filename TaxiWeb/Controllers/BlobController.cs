using AzureInterface;
using Contracts.Blob;
using Contracts.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TaxiWeb.ConfigModels;

namespace TaxiWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBussinesLogic authService;
        private readonly IBlob blob;
        public BlobController(IBussinesLogic authService, IBlob blob)
        {
            this.authService = authService;
            this.blob = blob;
        }


        [HttpPost]
        [AllowAnonymous]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [Route("upload-profile-image/{hashedEmail}")]
        public async Task<IActionResult> UploadProfileImage([FromForm] IFormFile file, [FromForm] string fileName)
        {
            Request.RouteValues.TryGetValue("hashedEmail", out object? email);

            if (email == null || file == null || file.Length == 0)
            {
                return BadRequest("Invalid parameters");
            }

            var fileExtenstion = fileName.Split('.')[1];
            var blobName = $"{email}.{fileExtenstion}";

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                var res = await this.blob.UploadBlob(blobName, memoryStream);
                if(res == null)
                {
                    return BadRequest("Failed to upload image");
                }
                return Ok(res);
            }
        }

        [HttpGet]
        // blobName = <some-hash>.<extension>
        [Route("get-image-sas/{blobName}")]
        public async Task<IActionResult> GetImageSas()
        {
            Request.RouteValues.TryGetValue("blobName", out object? blobName);

            if (blobName == null)
            {
                return BadRequest("Invalid parameters");
            }

            if (blobName is string blobNameString)
            {
                return Ok(await blob.GenerateSasUri(blobNameString));
            }

            return BadRequest("Invalid parameters");
        }
    }
}
