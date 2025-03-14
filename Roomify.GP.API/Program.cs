using Microsoft.EntityFrameworkCore;
using Roomify.GP.Core.Repositories.Contract;
using Roomify.GP.Core.Services.Contract;
using Roomify.GP.Repository.Repositories;
using Roomify.GP.Service.Services;
using Roomify.GP.Service.Mappings;
using Roomify.GP.Repository.Data.Contexts;
using Roomify.GP.Core.Service.Contract;
using Roomify.GP.Service.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Load appsettings.Local.json if exists

// Load appsettings.Local.json if exists
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add Swagger (for API testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
