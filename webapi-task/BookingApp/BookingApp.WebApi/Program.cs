using BookingApp.Infrastructure.Extensions;
using BookingApp.Infrastructure.Persistence;
using BookingApp.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

// TODO: Сервисы
// builder.Services.AddScoped<IPropertyService, PropertyService>();
// builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
// builder.Services.AddScoped<ISearchService, SearchService>();
// builder.Services.AddScoped<IReservationService, ReservationService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
    dbContext.Database.Migrate();

    BookingDbSeeder.Seed(dbContext);
}

app.UseSwagger();   
app.UseSwaggerUI(); 

app.MapControllers();

app.Run();