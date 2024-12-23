using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PromerceCRM.API.Identity;
using PromerceCRM.API.Models.Mapper;
using PromerceCRM.API.Repository;
using PromerceCRM.API.Repository.Implements;
using PromerceCRM.API.Repository.Interfaces;
using PromerceCRM.API.Services.Implements;
using PromerceCRM.API.Services.Interfaces;
using PromerceHelpDesk.Web.Services;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization()
        ;
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Db Context
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
#endregion

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

//Repo Services
builder.Services.AddTransient<DatabaseInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountGroupService, AccountGroupService>();
builder.Services.AddScoped<IIncidentService, IncidentService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISystemModuleService, SystemModuleService>();
builder.Services.AddScoped<IResolutionService, ResolutionService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

#region Identity
builder.Services.AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
});
# endregion

#region Authentication & JWT Config
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new TokenValidationParameters()
         {
             ValidateIssuer = false,
             ValidateAudience = false,
             ValidateLifetime = true,
             ValidIssuer = builder.Configuration["JwtOptions.ValidIssuer"],
             ValidAudience = builder.Configuration["JwtOptions.ValidAudience"],
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"])),
         };

     })
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // your login path
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Set the cookie expiration time
        options.SlidingExpiration = true; // Renew the expiration time if the user is active
    });

#endregion

#region Localization
const string defaultCulture = "ar";
var supportedCultures = new[]
   {
        new CultureInfo("en"),
        new CultureInfo("ar"),
    };
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
builder.Services.AddScoped<ISharedViewLocalizer, SharedViewLocalizer>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLocalization();
app.MapControllers();
app.MapRazorPages();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapRazorPages(); // Map Razor Pages
//    endpoints.MapControllers(); // Map API Controllers
//});

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ar-SA"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.Run();
