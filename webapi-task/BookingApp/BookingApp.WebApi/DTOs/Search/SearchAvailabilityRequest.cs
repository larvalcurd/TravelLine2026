namespace BookingApp.WebApi.DTOs.Search
{
    /// <summary>
    /// Query-параметры для поиска доступных вариантов размещения.
    /// </summary>
    public class SearchAvailabilityRequest
    {
        /// <summary>
        /// Город, в котором выполняется поиск.
        /// </summary>
        /// <example>Moscow</example>
        public required string City { get; set; }

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

        /// <summary>
        /// Количество гостей.
        /// </summary>
        /// <example>2</example>
        public int Guests { get; set; }

        /// <summary>
        /// Максимальная цена за ночь. Необязательный параметр.
        /// </summary>
        /// <example>6000</example>
        public decimal? MaxPrice { get; set; }
    }
}