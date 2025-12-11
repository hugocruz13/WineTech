using API.Services;
using Azure.Storage.Blobs;
using BLL.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// Adiciona autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Authority"];
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RoleClaimType = "https://isi.com/roles"
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Dependency injection dos seus serviços
builder.Services.AddServices();

// Adicionando Blob Storage
builder.Services.AddSingleton(x => new BlobServiceClient("UseDevelopmentStorage=true"));
builder.Services.AddScoped<IStorageService, BlobStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
