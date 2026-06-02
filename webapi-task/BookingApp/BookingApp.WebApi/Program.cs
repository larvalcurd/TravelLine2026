using BookingApp.Infrastructure.Extensions;
using BookingApp.WebApi.Extensions;
using BookingApp.WebApi.Middleware;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddWebApiServices();
builder.Services.AddDomainServices();
builder.Services.AddInfrastructure( builder.Configuration );

var app = builder.Build();

app.InitializeDatabase();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
