using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Получаем TempData
            var tempDataFactory = context.RequestServices.GetService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory.GetTempData(context);
            tempData["Exception"] = ex.Message; // Сохраняем сообщение об ошибке
            tempData.Save();
            Console.WriteLine($"Exception caught: {ex.Message}");
            // Перенаправляем на страницу ошибок
            context.Response.Redirect("/Home/Error");
            context.Response.StatusCode = StatusCodes.Status302Found;

            return Task.CompletedTask;
        }
    }
}
