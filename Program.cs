using System.Text.Json.Serialization;
using RealEstate.Middlewares;
using RealEstate.Repositories;
using RealEstate.Repositories.Interfaces;
using RealEstate.Services;
using RealEstate.Services.Interfaces;
using RealEstate.Utils;
using RealEstate.Utils.Interfaces;

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

// SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// UTILS
builder.Services.AddScoped<IJWT, RealEstate.Utils.JWT>();

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
