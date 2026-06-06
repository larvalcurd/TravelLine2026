namespace BookingApp.WebApi.DTOs.Reservations
{
    /// <summary>
    /// Представление бронирования в ответах API.
    /// </summary>
    public class ReservationResponse
    {
        /// <summary>
        /// Уникальный идентификатор бронирования.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор объекта размещения.
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Идентификатор категории номера.
        /// </summary>
        public Guid RoomTypeId { get; set; }

        /// <summary>
        /// Дата заезда.
        /// </summary>
        public DateOnly ArrivalDate { get; set; }

        /// <summary>
        /// Дата выезда.
        /// </summary>
        public DateOnly DepartureDate { get; set; }

        /// <summary>
        /// Время заезда.
        /// </summary>
        public TimeOnly ArrivalTime { get; set; }

        /// <summary>
        /// Время выезда.
        /// </summary>
        public TimeOnly DepartureTime { get; set; }

        /// <summary>
        /// ФИО гостя.
        /// </summary>
        public required string GuestName { get; set; }

        /// <summary>
        /// Контактный номер телефона гостя.
        /// </summary>
        public required string GuestPhoneNumber { get; set; }

        /// <summary>
        /// Количество гостей в бронировании.
        /// </summary>
        public int GuestCount { get; set; }

        /// <summary>
        /// Итоговая стоимость бронирования, рассчитанная сервером.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Валюта итоговой стоимости.
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// Признак отмененного бронирования.
        /// </summary>
        public bool IsCanceled { get; set; }
    }
}
