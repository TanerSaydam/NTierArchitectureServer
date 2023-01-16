using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NTierArchitectureServer.Business.MappingProfiles;
using NTierArchitectureServer.Business.Services.AuthServices;
using NTierArchitectureServer.Business.Services.AuthServices.Validators;
using NTierArchitectureServer.Business.Services.CategoryServices;
using NTierArchitectureServer.Business.Services.EmailSettingServices;
using NTierArchitectureServer.Business.Services.EmailSettingServices.Validators;
using NTierArchitectureServer.Business.Services.EmailTemplateServices;
using NTierArchitectureServer.Business.Services.Logs.LogCategoryServices;
using NTierArchitectureServer.Business.Services.UserServices;
using NTierArchitectureServer.Business.Services.UserServices.Validators;
using NTierArchitectureServer.Core.Exceptions;
using NTierArchitectureServer.Core.Security;
using NTierArchitectureServer.Core.Validation;
using NTierArchitectureServer.DataAccess.Context;
using NTierArchitectureServer.DataAccess.Repositories;
using NTierArchitectureServer.DataAccess.Repositories.CategoryRepository;
using NTierArchitectureServer.DataAccess.Repositories.EmailSettingRepository;
using NTierArchitectureServer.DataAccess.Repositories.EmailTemplateRepository;
using NTierArchitectureServer.DataAccess.Repositories.Logs.LogCategoryRepository;
using NTierArchitectureServer.Entities.Models;
using NTierArchitectureServer.Entities.Models.Identity;
using NTierArchitectureServer.Entities.Options;
using NTierArchitectureServer.WebApi.Options;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

#region DataAccess Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSettingRepository, EmailSettingRepository>();
builder.Services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ILogCategoryRepository, LogCategoryRepository>();
#endregion

#region Business Dependency Injection
builder.Services.AddScoped<IEmailSettingService,EmailSettingService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ILogCategoryService, LogCategoryService>();

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
#endregion

#region Core Dependency Injection
builder.Services.AddTransient<ExceptionHandler>();
builder.Services.AddScoped<ITokenHandler, TokenHandler>();
#endregion

#region ConfigureOptions
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
#endregion

#region Authentication
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
#endregion

#region DbContext
builder.Services.AddDbContext<AppDbContext>((sp,options) =>
{
    var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
    options.UseSqlServer(dbOptions.MSSqlConnectionString);
});
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.RequireUniqueEmail= true;    
}).AddEntityFrameworkStores<AppDbContext>();
#endregion

#region Application
#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddControllers(options=> options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration
    .RegisterValidatorsFromAssemblyContaining<RegisterValidator>()
    .RegisterValidatorsFromAssemblyContaining<LoginValidator>()
    .RegisterValidatorsFromAssemblyContaining<EmailSettingValidator>()
    .RegisterValidatorsFromAssemblyContaining<ChangeUserProfileImageValidator>()
    )
    .ConfigureApiBehaviorOptions(options=> options.SuppressModelStateInvalidFilter = true);
#pragma warning restore CS0618 // Type or member is obsolete
builder.Services.AddEndpointsApiExplorer();
#endregion

#region Log
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
        {
            new SqlColumn
            {
                ColumnName = "UserId",
                PropertyName = "UserId",
                DataType=SqlDbType.NVarChar,
                DataLength = 164,
                AllowNull = true,
            }
        }
};

Logger log = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
    connectionString: builder.Configuration.GetConnectionString("SqlServer"),
    tableName: "logs",
    autoCreateSqlTable: true,
    columnOptions: columnOptions
    ).CreateLogger();


builder.Host.UseSerilog(log);
#endregion

#region Swagger
builder.Services.AddSwaggerGen(setup =>
{
    

    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecuritySheme, Array.Empty<string>() }
    });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Set<EmailSetting>().Any())
    {
        EmailSetting emailSetting = new()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            Email = "tanersaydam@gmail.com",
            HTML = true,
            Password = "password",
            Port = 587,
            SMTP = "smtp.gmail.com",
            SSL = true,
        };

        db.Set<EmailSetting>().Add(emailSetting);
        db.SaveChanges();
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    if (!userManager.Users.Any())
        userManager.CreateAsync(new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "tanersaydam@gmail.com",
            UserName = "tanersaydam",
        }, "Password12*").Wait();

}

app.Use(async (context, next) =>
{
    var userId = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;

    IDisposable disposable = LogContext.PushProperty("UserId", userId);    
    await next();
});

app.UseMiddleware<ExceptionHandler>();

app.Run();
