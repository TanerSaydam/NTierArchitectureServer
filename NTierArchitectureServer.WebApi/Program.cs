using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTierArchitectureServer.DataAccess.Context;
using NTierArchitectureServer.Entities.Models.Identity;
using NTierArchitectureServer.Entities.Options;
using NTierArchitectureServer.WebApi.Options;

var builder = WebApplication.CreateBuilder(args);

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

app.Run();
