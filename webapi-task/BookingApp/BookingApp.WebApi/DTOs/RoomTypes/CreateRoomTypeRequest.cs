namespace BookingApp.WebApi.DTOs.RoomTypes
{
    /// <summary>
    /// Данные для создания категории номеров в объекте размещения.
    /// </summary>
    public class CreateRoomTypeRequest
    {
        /// <summary>
        /// Название категории номера.
        /// </summary>
        /// <example>Standard</example>
        public required string Name { get; set; } = string.Empty;

        /// <summary>
        /// Цена за одну ночь.
        /// </summary>
        /// <example>5000</example>
        public decimal DailyPrice { get; set; }

        /// <summary>
        /// Валюта цены в трехбуквенном формате.
        /// </summary>
        /// <example>RUB</example>
        public required string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Минимальное количество гостей для этой категории номера.
        /// </summary>
        /// <example>1</example>
        public int MinPersonCount { get; set; }

        /// <summary>
        /// Максимальное количество гостей для этой категории номера.
        /// </summary>
        /// <example>2</example>
        public int MaxPersonCount { get; set; }

        /// <summary>
        /// Общее количество физических номеров этой категории в объекте размещения.
        /// </summary>
        /// <example>10</example>
        public int TotalRoomsCount { get; set; }

        /// <summary>
        /// Сервисы, включенные в категорию номера.
        /// </summary>
        /// <example>["Breakfast"]</example>
        public IReadOnlyCollection<string> Services { get; set; } = [];

        /// <summary>
        /// Удобства, доступные в категории номера.
        /// </summary>
        /// <example>["Wi-Fi", "Air conditioning"]</example>
        public IReadOnlyCollection<string> Amenities { get; set; } = [];
    }
}