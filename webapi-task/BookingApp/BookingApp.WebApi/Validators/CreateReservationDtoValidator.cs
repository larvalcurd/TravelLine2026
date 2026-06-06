using BookingApp.WebApi.DTOs.Reservations;
using FluentValidation;
using System;

namespace BookingApp.WebApi.Validators
{
    public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
    {
        public CreateReservationRequestValidator()
        {
            RuleFor( x => x.PropertyId )
                .NotEmpty()
                .NotEqual( Guid.Empty )
                .WithMessage( "PropertyId is required and cannot be an empty GUID." );

            RuleFor( x => x.RoomTypeId )
                .NotEmpty()
                .NotEqual( Guid.Empty )
                .WithMessage( "RoomTypeId is required and cannot be an empty GUID." );

            // Проверка дат на дефолтное значение 0001-01-01
            RuleFor( x => x.ArrivalDate )
                .NotEmpty()
                .WithMessage( "ArrivalDate is required." );

            RuleFor( x => x.DepartureDate )
                .NotEmpty()
                .WithMessage( "DepartureDate is required." );

            RuleFor( x => x.ArrivalDate )
                .LessThan( x => x.DepartureDate )
                .WithMessage( "ArrivalDate must be earlier than DepartureDate." );

            // Проверка nullable-времени на то, что его вообще прислали (защита от null)
            RuleFor( x => x.ArrivalTime )
                .NotNull()
                .WithMessage( "ArrivalTime is required (e.g. '14:00:00')." );

            RuleFor( x => x.DepartureTime )
                .NotNull()
                .WithMessage( "DepartureTime is required (e.g. '12:00:00')." );

            RuleFor( x => x.GuestCount )
                .GreaterThan( 0 );

            RuleFor( x => x.GuestName )
                .NotEmpty()
                .MaximumLength( 300 );

            RuleFor( x => x.GuestPhoneNumber )
                .NotEmpty()
                .MaximumLength( 20 );
        }
    }
}
