using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MunchrBackend.Services;

namespace MunchrBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        private readonly BlobService _blobService;

        public BlobController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string fileName)
        {
            if (file == null || file.Length == 0) return BadRequest("Invalid file.");
						
            using var stream = file.OpenReadStream();
            var fileUrl = await _blobService.UploadFileAsync(stream, fileName); 

            Console.WriteLine($"File: {file?.FileName}");
Console.WriteLine($"FileName param: {fileName}");
						
            return Ok(new { FileUrl = fileUrl });
        }

    }
}