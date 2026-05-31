using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Search;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.ReservationApi
{
    [ApiController]
    [Route("api/search")]
    public class SearchController(ISearchService searchService) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IReadOnlyCollection<SearchAvailabilityResultDto>> Search([FromQuery] SearchAvailabilityQueryDto query)
        {
            var criteria = query.ToCriteria();
            var result = searchService.Search(criteria).Select(x => x.ToDto()).ToList();

            return Ok(result);
        }
    }
}