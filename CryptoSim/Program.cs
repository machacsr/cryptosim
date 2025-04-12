using CryptoSim.Dto;
using CryptoSim.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddDbContext<EventContext>(options => options.UseSqlServer("Server=(local);Database=CryptoDb_EGYP5A;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Filename=CryptoDb_EGYP5A.db"));

//TODO Services

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoSim API", Version = "v1" });
});

//TODO JWT


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ErrorlineSystem API v1"));
}

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();