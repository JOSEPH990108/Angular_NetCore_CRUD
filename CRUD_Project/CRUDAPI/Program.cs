using API.Extensions;
using AutoMapper;
using CRUDAPI.Data.DatabaseSeeds;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
//builder.Services.AddCors();

var app = builder.Build(); 

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try{
    var db = services.GetRequiredService<ApplicationDbContext>();
    var mapper = services.GetRequiredService<IMapper>();
    await db.Database.MigrateAsync();
    await Seed.SeedData(db, mapper);
}
catch(Exception ex){
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
} 

app.Run();
