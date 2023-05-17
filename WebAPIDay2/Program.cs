using EmployeeDep.BL;
using EmployeeDep.DAL;
using EmployeeDep.BL.Managers.Departments;
using EmployeeDep.DAL.Data.Context;
using EmployeeDep.DAL.Repos.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.AspNetCore.Identity;
using EmployeeDep.DAL.Data.Models;
using System.Security.Claims;


#region Default services
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = "Server=localhost;Port=5433;Database=WebAPIDay2EmpDept;User Id=postgres;Password=0000;";
builder.Services.AddDbContext<CompanyContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IDepartmentsManager, DepartmentsManager>();
builder.Services.AddScoped<IDepartmentsRepo, DepartmentsRepo>();

#endregion

#region Cors
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
#endregion

#region Identity

// Using ASPnetcore Identity package to manage identities, Declaring Identity Role (User) 
// and Database (CompanyContext), must be before Authentication so it doesn't get overwritten
builder.Services.AddIdentity<Employee, IdentityRole>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

})
    .AddEntityFrameworkStores<CompanyContext>();
#endregion


#region Authetication
// Validating with authetication and challenge scheme (same scheme)
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "MyAuthenticationScheme";
    option.DefaultChallengeScheme = "MyAuthenticationScheme";

}).AddJwtBearer("MyAuthenticationScheme", options =>
{
    // Convert Secret Hashing Key To ByteArray and encrypting it as SymmetricSecurityKey
    string keyString = builder.Configuration.GetValue<string>("SecretKey")!;
    byte[] keyInByteArray = Encoding.ASCII.GetBytes(keyString);
    var key = new SymmetricSecurityKey(keyInByteArray);

    options.TokenValidationParameters = new TokenValidationParameters 
    { 
        IssuerSigningKey = key,
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagersOnly", policy =>
    policy.RequireClaim(ClaimTypes.Role, "Manager", "CEO"));
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
