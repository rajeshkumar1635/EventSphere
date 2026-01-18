using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication setup

var jwtKey = builder.Configuration["Jwt:Key"];
var keyBytes = Encoding.UTF8.GetBytes(jwtKey!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });
builder.Services.AddAuthorization();
// ---------------- YARP Reverse Proxy ----------------
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Map YARP routes
app.MapReverseProxy();

app.Run();
