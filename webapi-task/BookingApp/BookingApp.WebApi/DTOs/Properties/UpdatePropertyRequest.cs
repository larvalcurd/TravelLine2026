namespace BookingApp.WebApi.DTOs.Properties
{
    /// <summary>
    /// Данные для обновления объекта размещения.
    /// </summary>
    public class UpdatePropertyRequest
    {
        /// <summary>
        /// Новое название объекта размещения.
        /// </summary>
        /// <example>Grand Test Hotel</example>
        public required string Name { get; set; }

        /// <summary>
        /// Страна, в которой расположен объект размещения.
        /// </summary>
        /// <example>Russia</example>
        public required string Country { get; set; }

        /// <summary>
        /// Город, в котором расположен объект размещения.
        /// </summary>
        /// <example>Moscow</example>
        public required string City { get; set; }

        /// <summary>
        /// Точный адрес объекта размещения.
        /// </summary>
        /// <example>Tverskaya 1</example>
        public required string Address { get; set; }

        /// <summary>
        /// Географическая широта. Допустимый диапазон: от -90 до 90.
        /// </summary>
        /// <example>55.7558</example>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Географическая долгота. Допустимый диапазон: от -180 до 180.
        /// </summary>
        /// <example>37.6173</example>
        public decimal Longitude { get; set; }
    }
}