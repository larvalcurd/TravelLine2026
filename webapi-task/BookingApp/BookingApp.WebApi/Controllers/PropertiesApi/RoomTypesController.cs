using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.RoomTypes;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.PropertiesApi
{
    /// <summary>
    /// Управляет категориями номеров объектов размещения.
    /// </summary>
    [ApiController]
    [Produces( "application/json" )]
    public class RoomTypesController( IRoomTypeService roomTypeService ) : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService = roomTypeService;

        /// <summary>
        /// Возвращает все категории номеров указанного объекта размещения.
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта размещения.</param>
        /// <returns>Список категорий номеров объекта размещения.</returns>
        /// <response code="200">Список категорий номеров успешно получен.</response>
        /// <response code="404">Объект размещения с указанным идентификатором не найден.</response>
        [HttpGet( "api/properties/{propertyId:guid}/roomtypes" )]
        [ProducesResponseType( typeof( IReadOnlyCollection<RoomTypeDto> ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public async Task<ActionResult<IReadOnlyCollection<RoomTypeDto>>> GetByPropertyId( Guid propertyId )
        {
            var roomTypes = await _roomTypeService.GetByPropertyIdAsync( propertyId );
            var response = roomTypes.Select( rt => rt.ToDto() ).ToList();
            return Ok( response );
        }

        /// <summary>
        /// Создает новую категорию номеров для указанного объекта размещения.
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта размещения.</param>
        /// <param name="dto">Данные создаваемой категории номеров.</param>
        /// <returns>Созданная категория номеров.</returns>
        /// <response code="201">Категория номеров успешно создана.</response>
        /// <response code="400">Переданы некорректные данные категории номеров.</response>
        /// <response code="404">Объект размещения с указанным идентификатором не найден.</response>
        [HttpPost( "api/properties/{propertyId:guid}/roomtypes" )]
        [Consumes( "application/json" )]
        [ProducesResponseType( typeof( RoomTypeDto ), StatusCodes.Status201Created )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public ActionResult<RoomTypeDto> Create( Guid propertyId, [FromBody] CreateRoomTypeDto dto )
        {
            var request = dto.ToCreateRequest();
            var created = _roomTypeService.Create( propertyId, request );

            return CreatedAtAction( nameof( GetById ), new { id = created.Id }, created.ToDto() );
        }

        /// <summary>
        /// Возвращает категорию номера по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории номера.</param>
        /// <returns>Данные категории номера.</returns>
        /// <response code="200">Категория номера найдена.</response>
        /// <response code="404">Категория номера с указанным идентификатором не найдена.</response>
        [HttpGet( "api/roomtypes/{id:guid}" )]
        [ProducesResponseType( typeof( RoomTypeDto ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public async Task<ActionResult<RoomTypeDto>> GetById( Guid id )
        {
            var roomType = await _roomTypeService.GetByIdAsync( id );
            return Ok( roomType.ToDto() );
        }

        /// <summary>
        /// Обновляет категорию номера.
        /// </summary>
        /// <param name="id">Идентификатор обновляемой категории номера.</param>
        /// <param name="dto">Новые данные категории номера.</param>
        /// <response code="204">Категория номера успешно обновлена.</response>
        /// <response code="400">Переданы некорректные данные категории номера.</response>
        /// <response code="404">Категория номера с указанным идентификатором не найдена.</response>
        [HttpPut( "api/roomtypes/{id:guid}" )]
        [Consumes( "application/json" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public IActionResult Update( Guid id, [FromBody] UpdateRoomTypeDto dto )
        {
            var request = dto.ToUpdateRequest();
            _roomTypeService.Update( id, request );

            return NoContent();
        }

        /// <summary>
        /// Удаляет категорию номера.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой категории номера.</param>
        /// <response code="204">Категория номера успешно удалена.</response>
        /// <response code="400">Категорию номера нельзя удалить из-за связанных бронирований.</response>
        /// <response code="404">Категория номера с указанным идентификатором не найдена.</response>
        [HttpDelete( "api/roomtypes/{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public IActionResult Delete( Guid id )
        {
            _roomTypeService.Delete( id );
            return NoContent();
        }
    }
}
