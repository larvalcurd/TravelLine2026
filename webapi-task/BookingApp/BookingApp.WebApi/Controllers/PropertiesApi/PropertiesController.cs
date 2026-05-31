using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Properties;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.PropertiesApi
{
    [ApiController]
    [Route("api/properties")]
    public class PropertiesController(IPropertyService propertyService) : ControllerBase
    {
        private readonly IPropertyService _propertyService = propertyService;

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<PropertyDto>), StatusCodes.Status200OK)]
        public ActionResult<IReadOnlyCollection<PropertyDto>> GetAll()
        {
            var properties = _propertyService.GetAll();
            var response = properties.Select(p => p.ToDto()).ToList();
            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PropertyDto> GetById(Guid id)
        {
            var property = _propertyService.GetById(id);
            return Ok(property.ToDto());
        }

        [HttpPost]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PropertyDto> Create([FromBody] CreatePropertyDto dto)
        {
            var propertyEntity = dto.ToEntity();
            var created = _propertyService.Create(propertyEntity);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(Guid id, [FromBody] UpdatePropertyDto dto)
        {
            var propertyEntity = dto.ToEntity(id); // Маппинг Update DTO -> Entity
            _propertyService.Update(id, propertyEntity);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            _propertyService.Delete(id);
            return NoContent();
        }
    }
}