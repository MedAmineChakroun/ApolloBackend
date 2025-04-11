using ApolloBackend.Configurations;
using ApolloBackend.Data;
using ApolloBackend.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApolloBackend.Services;
using ApolloBackend.Functions;
using ApolloBackend.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ERPContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ErpConnection")));


// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy => policy.WithOrigins("http://localhost:4200") // ✅ Replace with your Angular URL
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()); // Optional if using authentication cookies
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//declare Services
builder.Services.AddScoped<ProduitsFunctions>();
builder.Services.AddScoped<FamilleFunctions>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IDocumentVente, DocumentVenteFunctions>();
builder.Services.AddScoped<IDocumentVenteLigne, DocumentVenteLigneFunctions>();
//auths and jwt configs
// Add Identity with custom User class
builder.Services.AddIdentity<User, IdentityRole>(options =>
{

    // Password policy configuration
    options.Password.RequireDigit = true; // Require a digit
    options.Password.RequireLowercase = true; // Require lowercase letter
    options.Password.RequireUppercase = true; // Require uppercase letter
    options.Password.RequireNonAlphanumeric = true; // Require a non-alphanumeric character (symbol)
    options.Password.RequiredLength = 8; // Minimum length of the password (increased for security)

    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,//true for extra security
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true,

    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

//set application Roles 
// Inside `app.UseHttpsRedirection()
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await RoleInitializer.InitializeRoles(serviceProvider);
}
//;` before `app.UseAuthentication();`

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();