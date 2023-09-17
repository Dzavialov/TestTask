using EmailWebApi.Extensions;
using EmailWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IAzureBlobService _service;

        public FileController(IAzureBlobService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file, [FromForm] string userEmail)
        {
            if (!userEmail.IsValidEmail()) return BadRequest("Email address is not valid.");

            if (file.IsDocxExtension())
            {
                await _service.UploadFilesAsync(file, userEmail);
                return Ok();
            }

            return BadRequest("File format is invalid.");
        }
    }
}
