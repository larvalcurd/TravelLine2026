using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public class ReservationFilter
    {
        public Guid? PropertyId { get; set; }
        public Guid? RoomTypeId { get; set; }
        public DateOnly? ArrivalDateFrom { get; set; }
        public DateOnly? ArrivalDateTo { get; set; }
        public string? GuestName { get; set; }
        public string? GuestPhoneNumber { get; set; }
        public bool IncludeCanceled { get; set; }
    }
}