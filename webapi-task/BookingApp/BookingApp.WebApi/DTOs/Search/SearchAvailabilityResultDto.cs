namespace BookingApp.WebApi.DTOs.Search
{
    /// <summary>
    /// Доступный вариант размещения, найденный по параметрам поиска.
    /// </summary>
    public class SearchAvailabilityResultDto
    {
        /// <summary>
        /// Идентификатор объекта размещения.
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Название объекта размещения.
        /// </summary>
        public required string PropertyName { get; set; }

        /// <summary>
        /// Страна объекта размещения.
        /// </summary>
        public required string Country { get; set; }

        /// <summary>
        /// Город объекта размещения.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// Адрес объекта размещения.
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Географическая широта объекта размещения.
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Географическая долгота объекта размещения.
        /// </summary>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Идентификатор категории номера.
        /// </summary>
        public Guid RoomTypeId { get; set; }

        /// <summary>
        /// Название категории номера.
        /// </summary>
        public required string RoomTypeName { get; set; }

        /// <summary>
        /// Цена за одну ночь.
        /// </summary>
        public decimal DailyPrice { get; set; }

        /// <summary>
        /// Валюта цены.
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// Минимальное количество гостей для этой категории номера.
        /// </summary>
        public int MinPersonCount { get; set; }

        /// <summary>
        /// Максимальное количество гостей для этой категории номера.
        /// </summary>
        public int MaxPersonCount { get; set; }

        /// <summary>
        /// Количество свободных номеров этой категории на запрошенный период.
        /// </summary>
        public int AvailableRoomsCount { get; set; }

        /// <summary>
        /// Количество ночей между датой заезда и датой выезда.
        /// </summary>
        public int Nights { get; set; }

        /// <summary>
        /// Итоговая стоимость проживания за весь период.
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Сервисы, включенные в категорию номера.
        /// </summary>
        public IReadOnlyCollection<string> Services { get; set; } = [];

        /// <summary>
        /// Удобства, доступные в категории номера.
        /// </summary>
        public IReadOnlyCollection<string> Amenities { get; set; } = [];
    }
}