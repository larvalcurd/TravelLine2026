using BookingApp.WebApi.DTOs.RoomTypes;
using FluentValidation;

namespace BookingApp.WebApi.Validators
{
    public class CreateRoomTypeDtoValidator : AbstractValidator<CreateRoomTypeDto>
    {
        public CreateRoomTypeDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DailyPrice).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty().Length(3);
            RuleFor(x => x.MinPersonCount).GreaterThan(0);

            RuleFor(x => x.MaxPersonCount)
                .GreaterThanOrEqualTo(x => x.MinPersonCount)
                .WithMessage("MaxPersonCount must be greater than or equal to MinPersonCount.");

            RuleFor(x => x.TotalRoomsCount).GreaterThan(0);

            RuleFor(x => x.Services).NotNull();
            RuleFor(x => x.Amenities).NotNull();
        }
    }
}
