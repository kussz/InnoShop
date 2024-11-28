using InnoShop.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InnoShop.Frontend.Middleware
{
    public class HostSettingsFilter : ActionFilterAttribute
    {
        private readonly HostSettings _settings;

        public HostSettingsFilter(HostSettings settings)
        {
            _settings = settings;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                controller.ViewBag.Host = _settings; // Устанавливаем ViewBag.Host
            }
            base.OnActionExecuting(context);
        }
    }
}
