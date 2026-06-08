using BookingApp.WebApi.DTOs.Search;
using FluentValidation;

namespace BookingApp.WebApi.Validators
{
    public class SearchAvailabilityRequestValidator : AbstractValidator<SearchAvailabilityRequest>
    {
        public SearchAvailabilityRequestValidator()
        {
            RuleFor( x => x.City )
                .NotEmpty()
                .MaximumLength( 100 );

            RuleFor( x => x.ArrivalDate )
                .NotEmpty()
                .WithMessage( "ArrivalDate is required." );

            RuleFor( x => x.DepartureDate )
                .NotEmpty()
                .WithMessage( "DepartureDate is required." );

            RuleFor( x => x.ArrivalDate )
                .LessThan( x => x.DepartureDate )
                .WithMessage( "ArrivalDate must be earlier than DepartureDate." );

            RuleFor( x => x.Guests )
                .GreaterThan( 0 );

            RuleFor( x => x.MaxPrice )
                .GreaterThan( 0 )
                .When( x => x.MaxPrice.HasValue )
                .WithMessage( "MaxPrice must be greater than zero if specified." );
        }
    }
}
