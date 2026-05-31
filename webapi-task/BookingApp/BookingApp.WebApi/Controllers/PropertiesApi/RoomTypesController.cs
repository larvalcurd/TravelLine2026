using BookingApp.Domain.Interfaces.Services;
using BookingApp.WebApi.DTOs.RoomTypes;
using BookingApp.WebApi.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.WebApi.Controllers.PropertiesApi
{
    [ApiController]
    [Produces("application/json")]
    public class RoomTypesController(IRoomTypeService roomTypeService) : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService = roomTypeService;

        [HttpGet("api/properties/{propertyId:guid}/roomtypes")]
        [ProducesResponseType(typeof(IReadOnlyCollection<RoomTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IReadOnlyCollection<RoomTypeDto>> GetByPropertyId(Guid propertyId)
        {
            var roomTypes = _roomTypeService.GetByPropertyId(propertyId);
            var response = roomTypes.Select(rt => rt.ToDto()).ToList();
            return Ok(response);
        }

        [HttpPost("api/properties/{propertyId:guid}/roomtypes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(RoomTypeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RoomTypeDto> Create(Guid propertyId, [FromBody] CreateRoomTypeDto dto)
        {
            var roomTypeEntity = dto.ToEntity(propertyId);
            var created = _roomTypeService.Create(propertyId, roomTypeEntity);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created.ToDto());
        }

        [HttpGet("api/roomtypes/{id:guid}")]
        [ProducesResponseType(typeof(RoomTypeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RoomTypeDto> GetById(Guid id)
        {
            var roomType = _roomTypeService.GetById(id);
            return Ok(roomType.ToDto());
        }

        [HttpPut("api/roomtypes/{id:guid}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(Guid id, [FromBody] UpdateRoomTypeDto dto)
        {
            var existing = _roomTypeService.GetById(id);
            var roomTypeEntity = dto.ToEntity(id, existing.PropertyId);
            _roomTypeService.Update(id, roomTypeEntity);
            return NoContent();
        }

        [HttpDelete("api/roomtypes/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            _roomTypeService.Delete(id);
            return NoContent();
        }
    }
}