using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private const string _filePath = "files";

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var files = Directory.GetFiles(_filePath);
            return await Task.Run(() => Ok(files));
        }

        [HttpGet("{fileName}")]
        public IActionResult GetByName(string fileName)
        {
            try
            {
                Stream stream = new FileStream(Path.Combine(_filePath, fileName), FileMode.Open, FileAccess.Read);

                return File(stream, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file)
        {
            string path = Path.Combine(_filePath, file.FileName);
            using FileStream filestream = System.IO.File.Create(path);
            await file.CopyToAsync(filestream);
            filestream.Flush();
            return Ok($"File {path} created successfully");
        }
    }
}
