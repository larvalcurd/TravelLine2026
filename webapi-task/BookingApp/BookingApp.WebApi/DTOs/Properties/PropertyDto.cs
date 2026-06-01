namespace BookingApp.WebApi.DTOs.Properties
{
    /// <summary>
    /// Представление объекта размещения в ответах API.
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// Уникальный идентификатор объекта размещения.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название объекта размещения.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Страна, в которой расположен объект размещения.
        /// </summary>
        public required string Country { get; set; }

        /// <summary>
        /// Город, в котором расположен объект размещения.
        /// </summary>
        public required string City { get; set; }

        /// <summary>
        /// Точный адрес объекта размещения.
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Географическая широта.
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Географическая долгота.
        /// </summary>
        public decimal Longitude { get; set; }
    }
}