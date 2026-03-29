using JCT_Tracking_Api.Implementation;
using JCT_Tracking_Api.Interface;
using JCT_Tracking_Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Net;
using System.Text;
using Vessel_Tracking_Api.Enitity_Framework;
using Vessel_Tracking_Api.Middleware;
using Vessel_Tracking_Api.Models;
using Vessel_Tracking_Api.Services;




var builder = WebApplication.CreateBuilder(args);

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 20
    )
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<JwtService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IContainerRepository, ContainerRepository>();
builder.Services.AddScoped<IVessselRepository, VesselRepository>();

builder.Services.AddDbContext<TrackingDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleDb")));

var config = builder.Configuration.GetSection("ApiSecurity");

var key = Encoding.UTF8.GetBytes(config["JwtSecret"]);

builder.Services.AddSwaggerGen(options =>
{
    // API KEY configuration
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Enter API Key",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-KEY",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = config["Issuer"],
        ValidAudience = config["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = async context =>
        {
            context.Response.ContentType = "application/json";

            string message = "Token invalid.";

            if (context.Exception is SecurityTokenExpiredException)
            {
                message = "Token expired.";
            }

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                success = false,
                message = message,
                data = (object)null
            });
        },

        OnChallenge = async context =>
        {
            context.HandleResponse(); // prevent default 401 response

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            // Only send message if no response has been written
            if (!context.Response.HasStarted)
            {
                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Authentication failed.",
                    data = (object)null
                });
            }
        }
    };
});

builder.Services.AddAuthorization();

var rateLimitSettings = builder.Configuration.GetSection("RateLimiting").Get<RateLimitSettings>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("ApiPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = rateLimitSettings.PermitLimit;
        limiterOptions.Window = TimeSpan.FromSeconds(rateLimitSettings.Window);
        limiterOptions.QueueLimit = rateLimitSettings.QueueLimit;
        limiterOptions.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);

    options.AssumeDefaultVersionWhenUnspecified = true;

    options.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers().RequireRateLimiting("ApiPolicy");

app.Run();
