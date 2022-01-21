using FileService.Api.Dtos;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace FileService.Api.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly IMongoCollection<Entities.File> _files;

        public FileRepository(IMongoDatabase database)
        {
            this._files = database.GetCollection<Entities.File>(MongoCollectionNames.Files);
        }

        public async Task AddAsync(Entities.File obj)
        {
            await this._files.InsertOneAsync(obj);
        }

        public async Task CreateFileAsync(CreateOrUpdateFileDto model)
        {
            var file = new Entities.File
            {
                Name = model.Name,
                ContenteBase64 = model.ContenteBase64
            };

            await this.AddAsync(file);
        }

        public async Task DeleteAsync(Expression<Func<Entities.File, bool>> predicate)
        {
            _ = await _files.DeleteOneAsync(predicate);
        }

        public async Task DeleteFileAsync(string id)
        {
            await this.DeleteAsync(x => x.Id == id);
        }

        public IQueryable<Entities.File> GetAll()
        {
            return this._files.AsQueryable();
        }

        public async Task<Entities.File> GetFileByIdAsync(string fileId)
        {
            return await this.GetSingleAsync(x => x.Id == fileId);
        }

        public async Task<Entities.File> GetSingleAsync(Expression<Func<Entities.File, bool>> predicate)
        {
            var filter = Builders<Entities.File>.Filter.Where(predicate);
            return (await this._files.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<Entities.File> UpdateAsync(Entities.File obj)
        {
            var filter = Builders<Entities.File>.Filter.Where(x => x.Id == obj.Id);

            var updateDefBuilder = Builders<Entities.File>.Update;
            var updateDef = updateDefBuilder.Combine(new UpdateDefinition<Entities.File>[]
            {
                updateDefBuilder.Set(x => x.Name, obj.Name),
                updateDefBuilder.Set(x => x.ContenteBase64, obj.ContenteBase64)
            });
            await this._files.FindOneAndUpdateAsync(filter, updateDef);

            return await this._files.FindOneAndReplaceAsync(x => x.Id == obj.Id, obj);
        }

        public async Task<Entities.File> UpdatefileAsync(string id, CreateOrUpdateFileDto model)
        {
            var file = new Entities.File
            {
                Id = id,
                Name = model.Name,
                ContenteBase64 = model.ContenteBase64
            };

            return await this.UpdateAsync(file);
        }
    }
}
