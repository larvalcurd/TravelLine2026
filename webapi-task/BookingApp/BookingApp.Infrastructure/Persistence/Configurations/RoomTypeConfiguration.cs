using System.Text.Json;
using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Infrastructure.Persistence.Configurations
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.ToTable("RoomTypes");
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Name).IsRequired().HasMaxLength(200);
            builder.Property(rt => rt.Currency).IsRequired().HasMaxLength(3);
            builder.Property(rt => rt.DailyPrice).IsRequired(); 
            builder.Property(rt => rt.MinPersonCount).IsRequired();
            builder.Property(rt => rt.MaxPersonCount).IsRequired();
            builder.Property(rt => rt.TotalRoomsCount).IsRequired();

            builder.Property(rt => rt.Services)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
                .Metadata.SetValueComparer(CreateStringListComparer());

            builder.Property(rt => rt.Amenities)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>())
                .Metadata.SetValueComparer(CreateStringListComparer());

            builder.HasIndex(rt => rt.PropertyId);
            builder.HasIndex(rt => new { rt.PropertyId, rt.DailyPrice });
        }

        private static ValueComparer<List<string>> CreateStringListComparer()
        {
            return new ValueComparer<List<string>>(
                (a, b) => a != null && b != null && a.SequenceEqual(b),
                c => c.Aggregate(0, (hash, item) => HashCode.Combine(hash, item != null ? item.GetHashCode() : 0)),
                c => c.ToList()
            );
        }
    }
}
