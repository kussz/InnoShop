namespace InnoShop.Frontend.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Перенаправление на страницу ошибок
                context.Response.Redirect($"/Home/Error?exception={Uri.EscapeDataString(ex.Message)}");
            }
        }
    }
}
