using BookingApp.WebApi.DTOs.Reservations;
using FluentValidation;

namespace BookingApp.WebApi.Validators
{
    public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationDtoValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("PropertyId is required and cannot be an empty GUID.");

            RuleFor(x => x.RoomTypeId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("RoomTypeId is required and cannot be an empty GUID.");

            RuleFor(x => x.ArrivalDate)
                .LessThan(x => x.DepartureDate)
                .WithMessage("ArrivalDate must be earlier than DepartureDate.");

            RuleFor(x => x.GuestCount)
                .GreaterThan(0);

            RuleFor(x => x.GuestName)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.GuestPhoneNumber)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}