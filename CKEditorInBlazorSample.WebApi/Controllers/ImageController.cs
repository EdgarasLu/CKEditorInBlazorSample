using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CKEditorInBlazorSample.WebApi.Controllers
{
    [Route("api/Images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    using var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName), FileMode.Create);
                    file.CopyTo(stream);
                    return Ok(new { url = $"https://localhost:44387/api/Images/{fileName}" });
                }
                else
                {
                    return BadRequest("File data not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> LoadFile(string fileName)
        {

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", fileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, @"application/octet-stream", Path.GetFileName(path));

        }

    }
}

