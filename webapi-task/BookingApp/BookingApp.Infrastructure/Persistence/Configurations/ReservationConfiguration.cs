using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Infrastructure.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.GuestName).IsRequired().HasMaxLength(300);
            builder.Property(r => r.GuestPhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(r => r.GuestCount).IsRequired();

            builder.Property(r => r.ArrivalDate).IsRequired();
            builder.Property(r => r.DepartureDate).IsRequired();
            builder.Property(r => r.ArrivalTime).IsRequired();
            builder.Property(r => r.DepartureTime).IsRequired();

            builder.Property(r => r.Total).IsRequired();
            builder.Property(r => r.Currency).IsRequired().HasMaxLength(3);

            builder.Property(r => r.IsCanceled).IsRequired().HasDefaultValue(false);

            builder.HasOne<Property>()
                .WithMany()
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<RoomType>()
                .WithMany()
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new
            {
                r.RoomTypeId,
                r.IsCanceled,
                r.ArrivalDate,
                r.DepartureDate
            }).HasDatabaseName("IX_Reservations_Availability");

            builder.HasIndex(r => r.PropertyId).HasDatabaseName("IX_Reservations_PropertyId");
            builder.HasIndex(r => r.IsCanceled).HasDatabaseName("IX_Reservations_IsCanceled");
        }
    }
}