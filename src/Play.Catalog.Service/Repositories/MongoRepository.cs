using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using System.Collections.Generic;

namespace Play.Catalog.Service.Repositories
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterDefinitionBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase mongoDatabase, string collectionName)
        {
            _mongoDatabase = mongoDatabase;
            dbCollection = _mongoDatabase.GetCollection<T>(collectionName);
        }



        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterDefinitionBuilder.Empty).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbCollection.Find(filterDefinitionBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync();

        }

        public async Task CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            await dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(nameof(entity));
            }
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(x => x.Id, entity.Id);
            await dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task RemoveAsync(Guid id)
        {
            FilterDefinition<T> filter = filterDefinitionBuilder.Eq(x => x.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }


    }
}