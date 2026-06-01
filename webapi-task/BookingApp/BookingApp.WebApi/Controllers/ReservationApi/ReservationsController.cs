using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Reservations;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.ReservationApi
{
    /// <summary>
    /// Управляет бронированиями: создание, просмотр, фильтрация и отмена.
    /// </summary>
    [ApiController]
    [Route( "api/reservations" )]
    [Produces( "application/json" )]
    public class ReservationsController( IReservationService reservationService ) : ControllerBase
    {
        private readonly IReservationService _reservationService = reservationService;

        /// <summary>
        /// Создает новое бронирование.
        /// </summary>
        /// <param name="dto">Данные создаваемого бронирования.</param>
        /// <returns>Созданное бронирование с рассчитанной итоговой стоимостью.</returns>
        /// <response code="201">Бронирование успешно создано.</response>
        /// <response code="400">Переданы некорректные данные или категория номера не принадлежит указанному объекту размещения.</response>
        /// <response code="404">Объект размещения или категория номера не найдены.</response>
        /// <response code="409">На выбранный период нет свободных номеров указанной категории.</response>
        [HttpPost]
        [Consumes( "application/json" )]
        [ProducesResponseType( typeof( ReservationDto ), StatusCodes.Status201Created )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        [ProducesResponseType( StatusCodes.Status409Conflict )]
        public ActionResult<ReservationDto> Create( [FromBody] CreateReservationDto dto )
        {
            var request = dto.ToRequest();

            var createdReservation = _reservationService.Create( request );

            return CreatedAtAction(
                nameof( GetById ),
                new { id = createdReservation.Id },
                createdReservation.ToDto() );
        }

        /// <summary>
        /// Возвращает список бронирований с опциональной фильтрацией.
        /// </summary>
        /// <param name="filterDto">Параметры фильтрации бронирований.</param>
        /// <returns>Список бронирований, подходящих под фильтр.</returns>
        /// <response code="200">Список бронирований успешно получен.</response>
        /// <response code="400">Переданы некорректные параметры фильтрации.</response>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyCollection<ReservationDto> ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public ActionResult<IReadOnlyCollection<ReservationDto>> GetReservations( [FromQuery] ReservationFilterDto filterDto )
        {
            var reservationFilter = filterDto.ToFilter();

            var reservations = _reservationService.GetAll( reservationFilter );

            var response = reservations.Select( r => r.ToDto() ).ToList();
            return Ok( response );
        }

        /// <summary>
        /// Возвращает бронирование по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор бронирования.</param>
        /// <returns>Данные бронирования.</returns>
        /// <response code="200">Бронирование найдено.</response>
        /// <response code="404">Бронирование с указанным идентификатором не найдено.</response>
        [HttpGet( "{id:guid}" )]
        [ProducesResponseType( typeof( ReservationDto ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public ActionResult<ReservationDto> GetById( Guid id )
        {
            var reservation = _reservationService.GetById( id );
            return Ok( reservation.ToDto() );
        }

        /// <summary>
        /// Отменяет бронирование.
        /// </summary>
        /// <param name="id">Идентификатор отменяемого бронирования.</param>
        /// <response code="204">Бронирование успешно отменено.</response>
        /// <response code="404">Бронирование с указанным идентификатором не найдено.</response>
        [HttpDelete( "{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public IActionResult Cancel( Guid id )
        {
            _reservationService.Cancel( id );

            return NoContent();
        }
    }
}
