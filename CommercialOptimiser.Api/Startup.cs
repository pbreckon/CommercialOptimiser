using CommercialOptimiser.Data;
using CommercialOptimiser.Data.Tables;
using CommercialOptimiser.Api.Helpers;
using CommercialOptimiser.Api.Services;
using CommercialOptimiser.Api.Services.Contracts;
using CommercialOptimiser.Data.Factories;
using CommercialOptimiser.Data.Factories.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommercialOptimiser.Api
{
    public class Startup
    {
        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Public Methods

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDatabaseInitializer>();
                dbInitializer.Initialize();
                dbInitializer.SeedData();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();

            services.AddDbContext<DatabaseContext>();

            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<ITableModelConverter, TableModelConverter>();
            services.AddScoped<IOptimiserHelper, OptimiserHelper>();
            
            services.AddTransient<IBreakService, BreakService>();
            services.AddTransient<ICommercialService, CommercialService>(); 
            services.AddTransient<IUserBreakCommercialService, UserBreakCommercialService>();

            services.AddTransient<IBreakFactory, BreakFactory>();
            services.AddTransient<ICommercialFactory, CommercialFactory>();
            services.AddTransient<IUserReportBreakFactory, UserReportBreakFactory>();
        }
        
        #endregion
    }
}