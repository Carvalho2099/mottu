var builder = WebApplication.CreateBuilder(args);

// Logging structured configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options => {
    options.IncludeScopes = true;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
});

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection for repositories and services
builder.Services.AddScoped<VehicleManagement.Api.Repositories.Interfaces.IUserRepository, VehicleManagement.Api.Repositories.UserRepository>();
builder.Services.AddScoped<VehicleManagement.Api.Services.Interfaces.IAuthService, VehicleManagement.Api.Services.AuthService>();
builder.Services.AddScoped<VehicleManagement.Api.Repositories.Interfaces.IVehicleRepository, VehicleManagement.Api.Repositories.VehicleRepository>();
builder.Services.AddScoped<VehicleManagement.Api.Services.Interfaces.IVehicleService, VehicleManagement.Api.Services.VehicleService>();
builder.Services.AddSingleton<VehicleManagement.Api.Services.Interfaces.IMessageQueueService, VehicleManagement.Api.Services.RabbitMqService>();
builder.Services.AddScoped<VehicleManagement.Api.Repositories.Interfaces.IVehicleFileRepository, VehicleManagement.Api.Repositories.VehicleFileRepository>();
builder.Services.AddSingleton<VehicleManagement.Api.Services.MinioService>();

// JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "vehicle_management",
            ValidAudience = builder.Configuration["Jwt:Issuer"] ?? "vehicle_management",
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "super_secret_key"))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
