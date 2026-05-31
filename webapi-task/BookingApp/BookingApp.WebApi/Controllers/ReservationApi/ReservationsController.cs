using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Reservations;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.ReservationApi
{
    [ApiController]
    [Route("api/reservations")]
    [Produces("application/json")]
    public class ReservationsController(IReservationService reservationService) : ControllerBase
    {
        private readonly IReservationService _reservationService = reservationService;

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ReservationDto> Create([FromBody] CreateReservationDto dto)
        {
            var request = dto.ToRequest();

            var createdReservation = _reservationService.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdReservation.Id },
                createdReservation.ToDto());
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ReservationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IReadOnlyCollection<ReservationDto>> GetReservations([FromQuery] ReservationFilterDto filterDto)
        {
            var reservationFilter = filterDto.ToFilter();

            var reservations = _reservationService.GetAll(reservationFilter);

            var response = reservations.Select(r => r.ToDto()).ToList();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ReservationDto> GetById(Guid id)
        {
            var reservation = _reservationService.GetById(id);
            return Ok(reservation.ToDto());
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Cancel(Guid id)
        {
            _reservationService.Cancel(id);

            return NoContent();
        }
    }
}