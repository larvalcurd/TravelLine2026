namespace BookingApp.WebApi.DTOs.Reservations
{
    /// <summary>
    /// Данные для создания бронирования.
    /// </summary>
    public class CreateReservationRequest
    {
        /// <summary>
        /// Идентификатор объекта размещения.
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Идентификатор категории номера.
        /// </summary>
        public Guid RoomTypeId { get; set; }

        /// <summary>
        /// Дата заезда в формате YYYY-MM-DD.
        /// </summary>
        /// <example>2026-06-10</example>
        public DateOnly ArrivalDate { get; set; }

        /// <summary>
        /// Дата выезда в формате YYYY-MM-DD. Должна быть позже даты заезда.
        /// </summary>
        /// <example>2026-06-12</example>
        public DateOnly DepartureDate { get; set; }
        // Сделали nullable, чтобы отличать "забыли прислать" от "прислали полночь"

        /// <summary>
        /// Время заезда в формате HH:mm:ss. Nullable используется, чтобы отличать отсутствующее поле от полуночи.
        /// </summary>
        /// <example>14:00:00</example>
        public TimeOnly? ArrivalTime { get; set; }

        /// <summary>
        /// Время выезда в формате HH:mm:ss. Nullable используется, чтобы отличать отсутствующее поле от полуночи.
        /// </summary>
        /// <example>12:00:00</example>
        public TimeOnly? DepartureTime { get; set; }

        /// <summary>
        /// ФИО гостя.
        /// </summary>
        /// <example>Ivan Ivanov</example>
        public required string GuestName { get; set; }

        /// <summary>
        /// Контактный номер телефона гостя.
        /// </summary>
        /// <example>+79990000000</example>
        public required string GuestPhoneNumber { get; set; }

        /// <summary>
        /// Количество гостей в бронировании.
        /// </summary>
        /// <example>2</example>
        public int GuestCount { get; set; }
    }
}