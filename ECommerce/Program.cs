using System.Text;
using Data.Data;
using ECommerce.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Service.Implementation;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Logging: Serilog
// --------------------
builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

// --------------------
// Add core services
// --------------------
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger security definition for JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token with Bearer prefix (Example: 'Bearer eyJhbGciOi...')",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Enforce JWT authentication in Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// --------------------
// EF Core: Database Context
// --------------------
builder.Services.AddDbContext<EcommercedbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// --------------------
// Caching
// --------------------
builder.Services.AddMemoryCache();

// --------------------
// Dependency Injection: Services
// --------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// --------------------
// Authentication: JWT Bearer
// --------------------
var jwt = builder.Configuration.GetSection("Jwt");
if (!string.IsNullOrEmpty(jwt["Key"]))
{
    var key = Encoding.UTF8.GetBytes(jwt["Key"]);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
}

// --------------------
// CORS Policy
// --------------------
builder.Services.AddCors(o =>
    o.AddPolicy("open", p => p
        .WithOrigins("https://your-frontend.com") // Replace with your frontend domain
        .AllowAnyHeader()
        .AllowAnyMethod()));

// --------------------
// Build app
// --------------------
var app = builder.Build();

// --------------------
// Middleware pipeline
// --------------------
app.UseSerilogRequestLogging();

// If you are serving files locally (avatars, docs), keep this.
// If moving entirely to cloud (Azure Blob / S3), remove it.
app.UseStaticFiles();

// Custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("open");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
