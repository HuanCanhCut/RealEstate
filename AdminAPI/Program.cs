using System.Text.Json.Serialization;
using AdminAPI.Middlewares;
using AdminAPI.Repositories;
using AdminAPI.Repositories.Interfaces;
using AdminAPI.Services;
using AdminAPI.Services.Interfaces;
using AdminAPI.Utils;
using AdminAPI.Utils.Interfaces;

EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DATABASE
builder.Services.AddScoped<DbContext, DbContext>();

// SERVICES
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IContractSevice, ContractService>();

// REPOSITORIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IContractResponsitory, ContractResponsitory>();

// UTILS
builder.Services.AddScoped<IJWT, AdminAPI.Utils.JWT>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalError>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
