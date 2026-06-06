using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Search;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.ReservationApi
{
    /// <summary>
    /// Выполняет поиск доступных вариантов размещения по городу, датам, количеству гостей и цене.
    /// </summary>
    [ApiController]
    [Route( "api/search" )]
    [Produces( "application/json" )]
    public class SearchController( ISearchService searchService ) : ControllerBase
    {
        /// <summary>
        /// Возвращает доступные пары объект размещения / категория номера по заданным параметрам поиска.
        /// </summary>
        /// <param name="query">Параметры поиска доступности.</param>
        /// <returns>Список доступных вариантов размещения.</returns>
        /// <response code="200">Поиск выполнен успешно. Если вариантов нет, возвращается пустой список.</response>
        /// <response code="400">Переданы некорректные параметры поиска.</response>
        [HttpGet]
        [ProducesResponseType( typeof( IReadOnlyCollection<SearchAvailabilityResultDto> ), StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public async Task<ActionResult<IReadOnlyCollection<SearchAvailabilityResultDto>>> Search( [FromQuery] SearchAvailabilityQueryDto query )
        {
            var criteria = query.ToCriteria();
            var options = await searchService.SearchAsync( criteria );
            var result = options.Select( x => x.ToDto() ).ToList();

            return Ok( result );
        }
    }
}