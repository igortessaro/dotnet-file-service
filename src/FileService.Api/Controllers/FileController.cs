using FileService.Api.Dtos;
using FileService.Api.Mongo;
using FileService.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace FileService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;

        public FileController(MongoContext dbContext)
        {
            this._fileRepository = new FileRepository(dbContext.Database);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var files = this._fileRepository.GetAll().Select(x => new { x.Id, x.Name }).ToList();
            return await Task.Run(() => Ok(files));
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetByName(string fileName)
        {
            var file = await this._fileRepository.GetSingleAsync(x => x.Name == fileName);

            if (file == null)
            {
                return NotFound();
            }

            return File(Convert.FromBase64String(file.ContenteBase64), "application/octet-stream", file.Name);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();
            string contenteBase64 = Convert.ToBase64String(fileBytes);

            var fileToCreate = new CreateOrUpdateFileDto();
            fileToCreate.Name = file.FileName;
            fileToCreate.ContenteBase64 = contenteBase64;

            await this._fileRepository.CreateFileAsync(fileToCreate);

            return Ok($"File {file.FileName} created successfully");
        }
    }
}
