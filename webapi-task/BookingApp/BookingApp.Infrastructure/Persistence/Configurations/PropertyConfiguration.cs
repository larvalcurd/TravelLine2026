using BookingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Infrastructure.Persistence.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure( EntityTypeBuilder<Property> builder )
        {
            builder.ToTable( "Properties" );

            builder.HasKey( p => p.Id );

            builder.Property( p => p.Name ).IsRequired().HasMaxLength( 200 );

            builder.Property( p => p.Country ).IsRequired().HasMaxLength( 100 );
            builder.Property( p => p.City ).IsRequired().HasMaxLength( 100 );
            builder.Property( p => p.Address ).IsRequired().HasMaxLength( 500 );


            builder.Property( p => p.Latitude ).IsRequired().HasPrecision( 9, 6 );
            builder.Property( p => p.Longitude ).IsRequired().HasPrecision( 10, 6 );

            builder.HasMany( p => p.RoomTypes )
                .WithOne()
                .HasForeignKey( rt => rt.PropertyId )
                .OnDelete( DeleteBehavior.Restrict );

            builder.HasIndex( p => p.City );
        }
    }
}
