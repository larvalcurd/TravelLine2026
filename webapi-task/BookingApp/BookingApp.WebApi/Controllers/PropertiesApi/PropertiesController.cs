using BookingApp.Domain.Entities;
using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.Properties;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

/*
GET    /api/properties
GET    /api/properties/{id}
POST   /api/properties
PUT    /api/properties/{id}
DELETE /api/properties/{id}
*/

namespace BookingApp.WebApi.Controllers.PropertiesApi
{
    [ApiController]
    [Route("api/properties")]
    public class PropertiesController(IPropertyService propertyService) : ControllerBase
    {
        private readonly IPropertyService _propertyService = propertyService;

        // GET /api/properties
        [HttpGet]
        public ActionResult<IReadOnlyCollection<PropertyDto>> GetAll()
        {
            var properties = _propertyService.GetAll();
            var response = properties.Select(p => p.ToDto()).ToList();
            return Ok(response);
        }

        // GET /api/properties/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<PropertyDto> GetById(Guid id)
        {
            var property = _propertyService.GetById(id);
            return Ok(property.ToDto());
        }

        // POST /api/properties
        [HttpPost]
        public ActionResult<PropertyDto> Create([FromBody] CreatePropertyDto dto)
        {
            var propertyEntity = dto.ToEntity();
            var created = _propertyService.Create(propertyEntity);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        // PUT /api/properties/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdatePropertyDto dto)
        {
            var propertyEntity = dto.ToEntity(id); // Маппинг Update DTO -> Entity
            _propertyService.Update(id, propertyEntity);
            return NoContent();
        }

        // DELETE /api/properties/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            _propertyService.Delete(id);
            return NoContent();
        }
    }
}