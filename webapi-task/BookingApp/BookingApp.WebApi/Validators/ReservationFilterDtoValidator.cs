using BookingApp.WebApi.DTOs.Reservations;
using FluentValidation;

namespace BookingApp.WebApi.Validators
{
    public class ReservationFilterDtoValidator : AbstractValidator<ReservationFilterDto>
    {
        public ReservationFilterDtoValidator()
        {
            RuleFor( x => x.ArrivalDateFrom )
                .LessThanOrEqualTo( x => x.ArrivalDateTo!.Value )
                .When( x => x.ArrivalDateFrom.HasValue && x.ArrivalDateTo.HasValue )
                .WithMessage( "ArrivalDateFrom must be earlier than or equal to ArrivalDateTo." );

            RuleFor( x => x.GuestName )
                .MaximumLength( 300 )
                .When( x => !string.IsNullOrEmpty( x.GuestName ) );

            RuleFor( x => x.GuestPhoneNumber )
                .MaximumLength( 20 )
                .When( x => !string.IsNullOrEmpty( x.GuestPhoneNumber ) );

            RuleFor( x => x.PropertyId )
                .NotEqual( Guid.Empty )
                .When( x => x.PropertyId.HasValue )
                .WithMessage( "PropertyId cannot be an empty GUID." );

            RuleFor( x => x.RoomTypeId )
                .NotEqual( Guid.Empty )
                .When( x => x.RoomTypeId.HasValue )
                .WithMessage( "RoomTypeId cannot be an empty GUID." );
        }
    }
}
