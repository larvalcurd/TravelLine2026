using BookingApp.Domain.Extensions;
using BookingApp.Infrastructure.Extensions;
using BookingApp.WebApi.Extensions;
using BookingApp.WebApi.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddWebApiServices();
builder.Services.AddDomainServices();
builder.Services.AddInfrastructure( builder.Configuration );

WebApplication app = builder.Build();

app.InitializeDatabase();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
