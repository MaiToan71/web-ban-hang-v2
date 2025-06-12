using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Project.API;
using Project.Caching;
using Project.Caching.Interfaces;
using Project.Data.EF;
using Project.Data.Entities;
using Project.Services.Banks;
using Project.Services.BankUsers;
using Project.Services.Dashboards;
using Project.Services.Departments;
using Project.Services.Images;
using Project.Services.Menus;
using Project.Services.Notification;
using Project.Services.Orders;
using Project.Services.Posts;
using Project.Services.Posts.Interfaces;
using Project.Services.PostTypes;
using Project.Services.Products;
using Project.Services.Products.Interfaces;
using Project.Services.Schedule;
using Project.Services.Storages;
using Project.Services.TimeShifts;
using Project.Services.TimeShiftTypes;
using Project.Services.Users.Interfaces;
using Project.Usermager.Services;
using Project.Usermager.Services.Interfaces;
using Project.Utilities.AutoMapper;
using Project.Utilities.Constants;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
var isDevelopment = builder.Environment.IsDevelopment();
IConfigurationRoot configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

string AllowAll = "_AllowAll";
string vnderctApi = configurationBuilder["Domain:VnderctApi"];
string _domainApi = configurationBuilder["Domain:Api"];
string jwtSecretKey = configurationBuilder["JWTs:Key"];
string issuer = configurationBuilder["JWTs:Issuer"];
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(jwtSecretKey);


// Add services to the container.
// Add services to the container.
builder.Host.UseSerilog((hostingContext, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(hostingContext.Configuration));
builder.Services.AddControllers().AddJsonOptions(
    options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        // options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        // options.JsonSerializerOptions.WriteIndented = true;

    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ProductDbContext>(options =>
               options.UseSqlServer(configurationBuilder.GetConnectionString(SystemConstants.MainConnectionString)));
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ProductDbContext>().AddDefaultTokenProviders();
builder.Environment.IsDevelopment();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
     options.RequireHttpsMetadata = false;
     options.SaveToken = true;
     options.TokenValidationParameters = new TokenValidationParameters()
     {
         ValidateIssuer = true,
         ValidIssuer = issuer,
         ValidateAudience = true,
         ValidAudience = issuer,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ClockSkew = System.TimeSpan.Zero,
         IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
     };
 });

builder.Services.AddCors(options =>
{
    options.AddPolicy(
            name: AllowAll,
            builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }
        );
});

//Caching
builder.Services.AddSingleton<ICaching, Caching>();
builder.Services.AddSingleton<ICachingExtension, CachingExtension>();
// Auto Mapper Configurations
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

// Set a custom file upload limit (optional)
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024; // 50 MB
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 50 * 1024 * 1024; // 50 MB
});
builder.Services.AddControllersWithViews(); // hoặc
// builder.Services.AddMvc(); // (chứa cả Razor Pages và Controllers)

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
//Declare DI
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<IAppUserService, AppUserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddTransient<IPostTypeService, PostTypeService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<IImageService, ImageServcie>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IMenusService, MenuService>();
builder.Services.AddTransient<ITimeShiftService, TimeShiftService>();
builder.Services.AddTransient<ITimeShiftTypeService, TimeShiftTypeService>();
builder.Services.AddTransient<IBankService, BankService>();
builder.Services.AddTransient<IBankUserService, BankUserService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();
builder.Services.AddTransient<IDepartmentService, DeparmentService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IWorkScheduleService, WorkScheduleService>();
builder.Services.AddTransient<IProductService, ProductService>();
var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}



app.UseStaticFiles();




string newFolder = "UploadFiles";
string path = $"{System.IO.Directory.GetCurrentDirectory()}\\{newFolder}";

if (!System.IO.Directory.Exists(path))
{
    try
    {
        System.IO.Directory.CreateDirectory(path);
    }
    catch (IOException ie)
    {
        Console.WriteLine("IO Error: " + ie.Message);
    }
    catch (Exception e)
    {
        Console.WriteLine("General Error: " + e.Message);
    }
}



if (System.IO.Directory.Exists(path))
{
    app.UseStaticFiles(new StaticFileOptions
    {

        FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "UploadFiles")),
        RequestPath = "/UploadFiles"
    });
}


app.UseResponseCaching();
const string cacheMaxAge = "86400";
app.UseStaticFiles(new StaticFileOptions()
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.TryAdd("Cache-Control", $"public, max-age={cacheMaxAge}");
        if (ctx.File.Name.EndsWith(".js.gz"))
        {
            ctx.Context.Response.Headers["Content-Type"] = "text/javascript";
            ctx.Context.Response.Headers["Content-Encoding"] = "gzip";
        }
        if (ctx.File.Name.EndsWith(".css.gz"))
        {
            ctx.Context.Response.Headers["Content-Type"] = "text/css";
            ctx.Context.Response.Headers["Content-Encoding"] = "gzip";
        }
    },
});

//app.UseAntiXssMiddleware();
app.UseCors(x => x
          .AllowAnyMethod()
          .AllowAnyHeader()
          .SetIsOriginAllowed(origin => true)
          .AllowCredentials());
app.Environment.IsDevelopment();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
   ;


app.Run();
