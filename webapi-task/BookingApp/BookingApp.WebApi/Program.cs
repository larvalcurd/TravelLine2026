using System.Reflection;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.Domain.Services;
using BookingApp.Infrastructure.Extensions;
using BookingApp.Infrastructure.Persistence;
using BookingApp.Infrastructure.Persistence.Seed;
using BookingApp.WebApi.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly( typeof( Program ).Assembly );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options =>
{
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlFilePath = Path.Combine( AppContext.BaseDirectory, xmlFileName );
    options.IncludeXmlComments( xmlFilePath );
} );

builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddInfrastructure( builder.Configuration );

builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

var app = builder.Build();

using ( var scope = app.Services.CreateScope() )
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    dbContext.Database.Migrate();

    BookingDbSeeder.Seed( dbContext );
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();