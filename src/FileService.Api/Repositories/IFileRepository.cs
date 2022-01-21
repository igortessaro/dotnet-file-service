using FileService.Api.Dtos;

namespace FileService.Api.Repositories
{
    public interface IFileRepository : IRepository<Entities.File>
    {
        Task<Entities.File> GetFileByIdAsync(string fileId);

        Task CreateFileAsync(CreateOrUpdateFileDto model);

        Task<Entities.File> UpdatefileAsync(string id, CreateOrUpdateFileDto model);

        Task DeleteFileAsync(string id);
    }
}
