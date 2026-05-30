namespace BookingApp.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Пропускаем запрос дальше по конвейеру к контроллерам
                await _next(context);
            }
            catch (Exception ex)
            {
                // TODO: Вечером перехватить кастомные исключения (NotFoundException, ValidationException и т.д.)
                // TODO: Настроить правильные HTTP статус-коды (404, 400) и логирование

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
        }
    }
}
