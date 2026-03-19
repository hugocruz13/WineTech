using API.Filters.API.Filters;
using API.Hubs;
using API.Services;
using Azure.Storage.Blobs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.IdentityModel.Tokens.Jwt;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes.Add("bearerAuth", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Token"
        });

        document.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("bearerAuth"),
                    new List<string>()
                }
            }
        };

        document.SetReferenceHostDocument();
        return Task.CompletedTask;
    });
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var origins = (builder.Configuration["Cors:AllowedOrigins"] ?? "http://localhost:5173")
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Adiciona autentica��o JWT
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
            RoleClaimType = "https://winetech.pt/roles"
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs/notifications") ||
                     path.StartsWithSegments("/hubs/leituras")))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<EnsureUserExistsFilter>();
});

builder.Services.AddSignalR();

// SignalR
builder.Services.AddScoped<INotificationRealtimeService, NotificationRealtimeService>();
builder.Services.AddScoped<ILeituraRealtimeService, LeituraRealtimeService>();

// Registo do filtro no DI
builder.Services.AddScoped<EnsureUserExistsFilter>();

builder.Services.AddOpenApi();

// Dependency injection dos seus servi�os
builder.Services.AddServices();

// Adicionando Blob Storage
var blobConnectionString = builder.Configuration["BlobStorage:ConnectionString"] ?? "UseDevelopmentStorage=true";
builder.Services.AddSingleton(x => new BlobServiceClient(blobConnectionString));
builder.Services.AddScoped<IStorageService, BlobStorageService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var message = exception?.Message ?? "Erro interno inesperado.";

        await context.Response.WriteAsJsonAsync(new
        {
            success = false,
            message = $"Erro interno: {message}"
        });
    });
});

// SignalR
app.MapHub<NotificationHub>("/hubs/notifications");
app.MapHub<LeituraHub>("/hubs/leituras");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.MapScalarApiReference(); 
}

if (!builder.Configuration.GetValue("DisableHttpsRedirection", false))
{
    app.UseHttpsRedirection();
}

// CORS
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
