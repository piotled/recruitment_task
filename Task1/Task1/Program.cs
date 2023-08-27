using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Task1.Authorization;
using Task1.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<IdentityUser>(identityOptions =>
{
    identityOptions.User = new UserOptions()
    {
        RequireUniqueEmail = true
    };
    identityOptions.Lockout = builder.Configuration.GetSection("Lockout").Get<LockoutOptions>();
    identityOptions.Password = builder.Environment.IsDevelopment() 
    ? new PasswordOptions()
    {
        RequireDigit = false,
        RequiredLength = 0,
        RequiredUniqueChars = 0,
        RequireLowercase = false,
        RequireUppercase = false,
        RequireNonAlphanumeric = false,
    }
    : new PasswordOptions() { RequiredLength = 8 };
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
});

var tokenOptionsSection = builder.Configuration.GetSection(JwtTokenOptions.SectionName);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(jwtBearerOptions =>
{
    jwtBearerOptions.SaveToken = false;
    var tokenOptions = tokenOptionsSection.Get<JwtTokenOptions>();
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = tokenOptions.Audience,
        ValidIssuer = tokenOptions.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SigningKey))
    };
});
builder.Services.Configure<JwtTokenOptions>(tokenOptionsSection);
builder.Services.AddScoped<TokenManager>();
builder.Services.AddScoped<IAuthorizationHandler, TokenNotCancelledAuthorizationHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
