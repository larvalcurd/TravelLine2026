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
        /// <param name="request">Данные создаваемого бронирования.</param>
        /// <returns>Созданное бронирование с рассчитанной итоговой стоимостью.</returns>
        /// <response code="201">Бронирование успешно создано.</response>
        /// <response code="400">Переданы некорректные данные или категория номера не принадлежит указанному объекту размещения.</response>
        /// <response code="404">Объект размещения или категория номера не найдены.</response>
        /// <response code="409">На выбранный период нет свободных номеров указанной категории.</response>
        [HttpPost]
        [Consumes( "application/json" )]
        [ProducesResponseType( typeof( ReservationResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        [ProducesResponseType( StatusCodes.Status409Conflict )]
        public ActionResult<ReservationResponse> Create( [FromBody] CreateReservationRequest request )
        {
            var domainRequest = request.ToDomainRequest();

            var createdReservation = _reservationService.Create( domainRequest );

            return CreatedAtAction(
                nameof( GetById ),
                new { id = createdReservation.Id },
                createdReservation.ToResponse() );
        }

        /// <summary>
        /// Возвращает список бронирований с опциональной фильтрацией.
        /// </summary>
        /// <param name="request">Параметры фильтрации бронирований.</param>
        /// <returns>Список бронирований, подходящих под фильтр.</returns>
        /// <response code="200">Список бронирований успешно получен.</response>
        /// <response code="400">Переданы некорректные параметры фильтрации.</response>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyCollection<ReservationResponse> ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public async Task<ActionResult<IReadOnlyCollection<ReservationResponse>>> GetReservations( [FromQuery] ReservationFilterRequest request )
        {
            var reservationFilter = request.ToDomainFilter();

            var reservations = await _reservationService.GetAllAsync( reservationFilter );

            var response = reservations.Select( r => r.ToResponse() ).ToList();
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
        [ProducesResponseType( typeof( ReservationResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public async Task<ActionResult<ReservationResponse>> GetById( Guid id )
        {
            var reservation = await _reservationService.GetByIdAsync( id );
            return Ok( reservation.ToResponse() );
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
