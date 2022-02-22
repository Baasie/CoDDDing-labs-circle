using ExternalDependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SeatsSuggestions.Domain;
using SeatsSuggestions.Domain.Port;
using SeatsSuggestions.Infra.Adapter;

namespace SeatsSuggestions.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // api/data_for_auditoriumSeating/
            IProvideAuditoriumLayouts auditoriumSeatingRepository =
                new AuditoriumWebRepository("http://localhost:6000/");

            // data_for_reservation_seats/
            IProvideCurrentReservations seatReservationsProvider =
                new SeatReservationsWebRepository("http://localhost:5000/");
            var seatAllocator =
                new SeatAllocator(new AuditoriumSeatingAdapter(auditoriumSeatingRepository, seatReservationsProvider));
            services.AddSingleton<IProvideSeatSuggestionsForShows>(seatAllocator);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SeatsSuggestions API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SeatsSuggestions API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}