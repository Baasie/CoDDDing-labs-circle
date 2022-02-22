using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalDependencies;
using ExternalDependencies.AuditoriumLayoutRepository;
using ExternalDependencies.ReservationsProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SeatsSuggestions.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            IProvideAuditoriumLayouts auditoriumSeatingRepository = new AuditoriumWebRepository("http://localhost:6000/");
            // api/data_for_auditoriumSeating/

            IProvideCurrentReservations seatReservationsProvider = new SeatReservationsWebAdapter("http://localhost:5000/"); 
            // data_for_reservation_seats/

            services.AddSingleton<IProvideAuditoriumLayouts>(auditoriumSeatingRepository);
            services.AddSingleton<IProvideCurrentReservations>(seatReservationsProvider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
