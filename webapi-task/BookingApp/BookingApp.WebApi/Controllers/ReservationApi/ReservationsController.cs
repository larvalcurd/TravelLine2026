using System.ComponentModel.DataAnnotations;
using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Reservations;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

/*
POST   /api/reservations
GET    /api/reservations
GET    /api/reservations/{id}
DELETE /api/reservations/{id}
*/
namespace BookingApp.WebApi.Controllers.ReservationApi
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationsController(IReservationService reservationService) : ControllerBase
    {
        private readonly IReservationService _reservationService = reservationService;

        [HttpPost]
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
        public ActionResult<IReadOnlyCollection<ReservationDto>> GetReservations([FromQuery] ReservationFilterDto filterDto)
        {
            var reservationFilter = filterDto.ToFilter();

            var reservations = _reservationService.GetAll(reservationFilter);

            var response = reservations.Select(r => r.ToDto()).ToList();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<ReservationDto> GetById(Guid id)
        {
            var reservation = _reservationService.GetById(id);
            return Ok(reservation.ToDto());
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Cancel(Guid id)
        {
            _reservationService.Cancel(id);

            return NoContent();
        }
    }
}