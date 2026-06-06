namespace BookingApp.WebApi.DTOs.Reservations
{
    /// <summary>
    /// Query-параметры для фильтрации списка бронирований.
    /// </summary>
    public class ReservationFilterRequest
    {
        /// <summary>
        /// Фильтр по идентификатору объекта размещения.
        /// </summary>
        public Guid? PropertyId { get; set; }

        /// <summary>
        /// Фильтр по идентификатору категории номера.
        /// </summary>
        public Guid? RoomTypeId { get; set; }

        /// <summary>
        /// Начальная дата диапазона дат заезда.
        /// </summary>
        public DateOnly? ArrivalDateFrom { get; set; }

        /// <summary>
        /// Конечная дата диапазона дат заезда.
        /// </summary>
        public DateOnly? ArrivalDateTo { get; set; }

        /// <summary>
        /// Фрагмент ФИО гостя для поиска.
        /// </summary>
        public string? GuestName { get; set; }

        /// <summary>
        /// Фрагмент номера телефона гостя для поиска.
        /// </summary>
        public string? GuestPhoneNumber { get; set; }

        /// <summary>
        /// Если true, в результат включаются отмененные бронирования.
        /// </summary>
        public bool IncludeCanceled { get; set; }
    }
}