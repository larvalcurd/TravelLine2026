using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Properties;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.PropertiesApi
{
    /// <summary>
    /// Управляет объектами размещения: создание, просмотр, обновление и удаление.
    /// </summary>
    [ApiController]
    [Route( "api/properties" )]
    [Produces( "application/json" )]
    public class PropertiesController( IPropertyService propertyService ) : ControllerBase
    {
        private readonly IPropertyService _propertyService = propertyService;

        /// <summary>
        /// Возвращает список всех объектов размещения.
        /// </summary>
        /// <returns>Список объектов размещения.</returns>
        /// <response code="200">Список объектов размещения успешно получен.</response>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyCollection<PropertyResponse> ), StatusCodes.Status200OK )]
        public async Task<ActionResult<IReadOnlyCollection<PropertyResponse>>> GetAll()
        {
            var properties = await _propertyService.GetAllAsync();
            var response = properties.Select( p => p.ToResponse() ).ToList();
            return Ok( response );
        }

        /// <summary>
        /// Возвращает объект размещения по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта размещения.</param>
        /// <returns>Данные объекта размещения.</returns>
        /// <response code="200">Объект размещения найден.</response>
        /// <response code="404">Объект размещения с указанным идентификатором не найден.</response>
        [HttpGet( "{id:guid}" )]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public async Task<ActionResult<PropertyResponse>> GetById( Guid id )
        {
            var property = await _propertyService.GetByIdAsync( id );
            return Ok( property.ToResponse() );
        }

        /// <summary>
        /// Создает новый объект размещения.
        /// </summary>
        /// <param name="request">Данные создаваемого объекта размещения.</param>
        /// <returns>Созданный объект размещения.</returns>
        /// <response code="201">Объект размещения успешно создан.</response>
        /// <response code="400">Переданы некорректные данные объекта размещения.</response>
        [HttpPost]
        [Consumes( "application/json" )]
        [ProducesResponseType( typeof( PropertyResponse ), StatusCodes.Status201Created )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public ActionResult<PropertyResponse> Create( [FromBody] CreatePropertyRequest request )
        {
            var domainRequest = request.ToDomainRequest();
            var created = _propertyService.Create( domainRequest );

            return CreatedAtAction( nameof( GetById ), new { id = created.Id }, created.ToResponse() );
        }

        /// <summary>
        /// Обновляет объект размещения.
        /// </summary>
        /// <param name="id">Идентификатор обновляемого объекта размещения.</param>
        /// <param name="request">Новые данные объекта размещения.</param>
        /// <response code="204">Объект размещения успешно обновлен.</response>
        /// <response code="400">Переданы некорректные данные объекта размещения.</response>
        /// <response code="404">Объект размещения с указанным идентификатором не найден.</response>
        [HttpPut( "{id:guid}" )]
        [Consumes( "application/json" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public IActionResult Update( Guid id, [FromBody] UpdatePropertyRequest request )
        {
            var domainRequest = request.ToDomainRequest();
            _propertyService.Update( id, domainRequest );
            return NoContent();
        }

        /// <summary>
        /// Удаляет объект размещения.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого объекта размещения.</param>
        /// <response code="204">Объект размещения успешно удален.</response>
        /// <response code="400">Объект размещения нельзя удалить из-за связанных данных.</response>
        /// <response code="404">Объект размещения с указанным идентификатором не найден.</response>
        [HttpDelete( "{id:guid}" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public IActionResult Delete( Guid id )
        {
            _propertyService.Delete( id );
            return NoContent();
        }
    }
}