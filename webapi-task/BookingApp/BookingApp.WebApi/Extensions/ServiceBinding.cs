using System.Reflection;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace BookingApp.WebApi.Extensions
{
    public static class ServiceBinding
    {
        public static IServiceCollection AddWebApiServices( this IServiceCollection services )
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly( typeof( Program ).Assembly );

            services.AddSwaggerGen( options =>
            {
                string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlFilePath = Path.Combine( AppContext.BaseDirectory, xmlFileName );
                options.IncludeXmlComments( xmlFilePath );
            } );
            services.AddFluentValidationRulesToSwagger();

            return services;
        }

        public static IServiceCollection AddDomainServices( this IServiceCollection services )
        {
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IReservationService, ReservationService>();

            return services;
        }
    }
}