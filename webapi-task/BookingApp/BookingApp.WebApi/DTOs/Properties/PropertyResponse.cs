namespace BookingApp.WebApi.DTOs.Properties
{
    /// <summary>
    /// Представление объекта размещения в ответах API.
    /// </summary>
    public class PropertyResponse
    {
        /// <summary>
        /// Уникальный идентификатор объекта размещения.
        /// </summary>
        /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
        public Guid Id { get; set; }

        /// <summary>
        /// Название объекта размещения.
        /// </summary>
        /// <example>Grand Hotel</example>
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
        /// <example>Tverskaya Street, 1</example>
        public required string Address { get; set; }

        /// <summary>
        /// Географическая широта.
        /// </summary>
        /// <example>55.7558</example>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Географическая долгота.
        /// </summary>
        /// <example>37.6176</example>
        public decimal Longitude { get; set; }
    }
}