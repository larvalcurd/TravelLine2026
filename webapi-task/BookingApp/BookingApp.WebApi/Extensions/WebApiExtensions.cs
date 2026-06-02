using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

namespace BookingApp.WebApi.Extensions
{
    public static class WebApiExtensions
    {
        public static IServiceCollection AddWebApiServices( this IServiceCollection services )
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly( typeof( Program ).Assembly );

            services.AddSwaggerGen( options =>
            {
                var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFilePath = Path.Combine( AppContext.BaseDirectory, xmlFileName );
                options.IncludeXmlComments( xmlFilePath );
            } );
            services.AddFluentValidationRulesToSwagger();

            return services;
        }
    }
}
