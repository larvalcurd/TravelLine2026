using BookingApp.WebApi.DTOs.Properties;
using FluentValidation;

namespace BookingApp.WebApi.Validators
{
    public class CreatePropertyRequestValidator : AbstractValidator<CreatePropertyRequest>
    {
        public CreatePropertyRequestValidator()
        {
            RuleFor( x => x.Name ).NotEmpty().MaximumLength( 200 );
            RuleFor( x => x.Country ).NotEmpty().MaximumLength( 100 );
            RuleFor( x => x.City ).NotEmpty().MaximumLength( 100 );
            RuleFor( x => x.Address ).NotEmpty().MaximumLength( 500 );
            RuleFor( x => x.Latitude ).InclusiveBetween( -90m, 90m );
            RuleFor( x => x.Longitude ).InclusiveBetween( -180m, 180m );
        }
    }
}
