using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.RoomTypes;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

/*
GET    /api/properties/{propertyId}/roomtypes
POST   /api/properties/{propertyId}/roomtypes
GET    /api/roomtypes/{id}
PUT    /api/roomtypes/{id}
DELETE /api/roomtypes/{id}
*/

namespace BookingApp.WebApi.Controllers.PropertiesApi
{
    [ApiController]
    public class RoomTypesController(IRoomTypeService roomTypeService) : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService = roomTypeService;

        [HttpGet("api/properties/{propertyId:guid}/roomtypes")]
        public ActionResult<IReadOnlyCollection<RoomTypeDto>> GetByPropertyId(Guid propertyId)
        {
            var roomTypes = _roomTypeService.GetByPropertyId(propertyId);
            var response = roomTypes.Select(rt => rt.ToDto()).ToList();
            return Ok(response);
        }

        [HttpPost("api/properties/{propertyId:guid}/roomtypes")]
        public ActionResult<RoomTypeDto> Create(Guid propertyId, [FromBody] CreateRoomTypeDto dto)
        {
            var roomTypeEntity = dto.ToEntity(propertyId);
            var created = _roomTypeService.Create(propertyId, roomTypeEntity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        [HttpGet("api/roomtypes/{id:guid}")]
        public ActionResult<RoomTypeDto> GetById(Guid id)
        {
            var roomType = _roomTypeService.GetById(id);
            return Ok(roomType.ToDto());
        }

        [HttpPut("api/roomtypes/{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateRoomTypeDto dto)
        {
            var existing = _roomTypeService.GetById(id);
            var roomTypeEntity = dto.ToEntity(id, existing.PropertyId);
            _roomTypeService.Update(id, roomTypeEntity);
            return NoContent();
        }

        [HttpDelete("api/roomtypes/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            _roomTypeService.Delete(id);
            return NoContent();
        }
    }
}