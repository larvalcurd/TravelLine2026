using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Search;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.ReservationApi
{
    [ApiController]
    [Route("api/search")]
    [Produces("application/json")]
    public class SearchController(ISearchService searchService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<SearchAvailabilityResultDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IReadOnlyCollection<SearchAvailabilityResultDto>> Search([FromQuery] SearchAvailabilityQueryDto query)
        {
            var criteria = query.ToCriteria();
            var result = searchService.Search(criteria).Select(x => x.ToDto()).ToList();

            return Ok(result);
        }
    }
}