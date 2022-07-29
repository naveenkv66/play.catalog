using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton(x =>
            {
                var configuration = x.GetService<IConfiguration>();
                var serviceSettings = configuration.GetSection("ServiceSettings").Get<ServiceSettings>();

                var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

                var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
                return mongoClient.GetDatabase(serviceSettings.ServiceName);
            });

            services.AddSingleton<IRepository<Item>>(ServiceProvider =>
            {
                var mongoDB = ServiceProvider.GetService<IMongoDatabase>();
                return new MongoRepository<Item>(mongoDB, "items");
            });
            return services;

        }
    }
}