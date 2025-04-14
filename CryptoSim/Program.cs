using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;
using CryptoSim.Services;
using CryptoSim.Services.Exceptions;
using CryptoSim.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//builder.Services.AddDbContext<EventContext>(options => options.UseSqlServer("Server=(local);Database=CryptoDb_EGYP5A;Trusted_Connection=True;TrustServerCertificate=True;"));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Filename=CryptoDb_EGYP5A.db"));

//TODO Services
builder.Services.AddScoped<UserService, UserServiceImpl>();
builder.Services.AddScoped<WalletService, WalletServiceImpl>();
builder.Services.AddScoped<IUnitOfWork, BasicUnitOfWork>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} ${context.HttpContext.Request.Path}";
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});
builder.Services.AddExceptionHandler<BadRequestExceptionHandlerImpl>();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoSim API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        []
    }});
});

builder.Services.AddAuthorization();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:5177",
            ValidAudience = "https://localhost:5177",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("randomSztring12345_x2____randomSztring12345_x2")),
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ErrorlineSystem API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.UseExceptionHandler();

app.MapControllers();

app.Run();