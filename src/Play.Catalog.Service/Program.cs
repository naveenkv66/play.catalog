using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Settings;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Entities;

var builder = WebApplication.CreateBuilder(args);

var serviceSettings = builder.Configuration.GetSection("ServiceSettings").Get<ServiceSettings>();

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();



// Add services to the container.

builder.Services.AddMongoDb().AddMongoRepository();
builder.Services.AddControllers(option =>
{
    option.SuppressAsyncSuffixInActionNames = false;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
