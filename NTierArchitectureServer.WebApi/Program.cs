using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTierArchitectureServer.DataAccess.Context;
using NTierArchitectureServer.DataAccess.Repositories.EmailSettingRepository;
using NTierArchitectureServer.Entities.Models;
using NTierArchitectureServer.Entities.Models.Identity;
using NTierArchitectureServer.Entities.Options;
using NTierArchitectureServer.WebApi.Options;

var builder = WebApplication.CreateBuilder(args);

#region DataAccess Dependency Injection
builder.Services.AddScoped<IEmailSettingRepository, EmailSettingRepository>();
#endregion

#region ConfigureOptions
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
#endregion

#region DbContext
builder.Services.AddDbContext<AppDbContext>((sp,options) =>
{
    var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
    options.UseSqlServer(dbOptions.MSSqlConnectionString);
});
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<AppDbContext>();
#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.Run();
