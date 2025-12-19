using System.Text.Json.Serialization;
using AdminAPI.Middlewares;
using AdminAPI.Repositories;
using AdminAPI.Utils;
using AdminAPI.Utils.Interfaces;

EnvLoader.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DbContext, DbContext>();

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
