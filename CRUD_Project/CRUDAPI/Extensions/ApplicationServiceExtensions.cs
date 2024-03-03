using CRUDAPI.Data;
using CRUDAPI.Helpers;
using CRUDAPI.Interfaces;

//using CRUDAPI.Interfaces;
//using CRUDAPI.Data;
using Microsoft.EntityFrameworkCore;
//using CRUDAPI.Data.DatabaseSeeds;


namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
