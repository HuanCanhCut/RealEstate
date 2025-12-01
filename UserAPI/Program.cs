using System.Text.Json.Serialization;
using System.Xml;
using UserAPI.Middlewares;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
using UserAPI.Respositories;
using UserAPI.Respositories.Interfaces;
using UserAPI.Services;
using UserAPI.Services.Interfaces;
using UserAPI.Utils;
using UserAPI.Utils.Interfaces;

EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DbContext>(provider => new DbContext());

// REPOSITORIES
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();

// SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();

// UTILS
builder.Services.AddScoped<IJWT, UserAPI.Utils.JWT>();

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
