using System.Security.Authentication;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SchoolManagement.API.Data;
using backend.Interfaces.Services;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models.Entities;
using SchoolManagement.API.Services;
using SchoolManagement.Auth;
using Serilog;

// Create builder
var builder = WebApplication.CreateBuilder(args);

// ========== Configure Serilog ==========
Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.CreateLogger();



// ========== Configuration ==========
builder.Configuration
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddEnvironmentVariables();

// ========== Services ==========
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) =>
	configuration
		.ReadFrom.Configuration(context.Configuration)
		.ReadFrom.Services(services)
		.Enrich.FromLogContext()
		.WriteTo.Console()
		.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

// ========== Identity ==========
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 6;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;

	options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;

	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// ========== CORS ==========
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecificOrigin",
		policy => policy
			.WithOrigins("http://localhost:4200")
			.AllowAnyHeader()
			.AllowAnyMethod()
			.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
			.AllowCredentials());
});

// ========== Authentication ==========
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtIssuer,
		ValidAudience = jwtAudience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
	};
});

// ========== Dependency Injection ==========
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();

// ========== Kestrel HTTPS Protocols ==========
builder.WebHost.ConfigureKestrel(options =>
{
	options.ConfigureHttpsDefaults(httpsOptions =>
	{
		httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
	});
});

// Build app
var app = builder.Build();

// ========== Middleware ==========
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();

	var initializeRoles = app.Configuration.GetValue<bool>("Configuration:InitializeRoles");
	if (initializeRoles)
	{
		using var scope = app.Services.CreateScope();
		var services = scope.ServiceProvider;
		await RoleInitializer.CreateRoles(services);
	}
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Run app
try
{
	Log.Information("Application is starting");
	await app.RunAsync();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application startup failed: {Message}, {StackTrace}", ex.Message, ex.StackTrace);
	throw;
}
finally
{
	Log.CloseAndFlush();
}
