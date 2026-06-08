namespace BookingApp.WebApi.DTOs.RoomTypes
{
    /// <summary>
    /// Представление категории номеров в ответах API.
    /// </summary>
    public class RoomTypeResponse
    {
        /// <summary>
        /// Уникальный идентификатор категории номера.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор объекта размещения, которому принадлежит категория номера.
        /// </summary>
        public Guid PropertyId { get; set; }

        /// <summary>
        /// Название категории номера.
        /// </summary>
        public required string Name { get; set; }

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
        /// Общее количество физических номеров этой категории в объекте размещения.
        /// </summary>
        public int TotalRoomsCount { get; set; }

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