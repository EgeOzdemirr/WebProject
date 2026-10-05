using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebProject.WebUI.Conventions
{
    /// <summary>
    /// "DemoAdmin" rolündeki kullanıcı admin panelini gezebilir ama hiçbir şeyi değiştiremez:
    /// GET dışındaki tüm istekler ile (GET ile çalışan) silme / durum değiştirme action'ları engellenir.
    /// </summary>
    public class DemoReadOnlyFilter : IActionFilter
    {
        public const string Role = "DemoAdmin";
        public const string MessageKey = "DemoMessage";

        private static readonly Regex WriteActions =
            new(@"^(Delete|Remove|ChangeStatus|ChangeFeatureSlider)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (!user.IsInRole(Role) || user.IsInRole("Admin"))
            {
                return;
            }

            var request = context.HttpContext.Request;
            var action = context.RouteData.Values["action"]?.ToString() ?? string.Empty;
            var isRead = HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method);
            if (isRead && !WriteActions.IsMatch(action))
            {
                return;
            }

            if (context.Controller is Controller controller)
            {
                controller.TempData[MessageKey] = "Demo modunda değişiklik yapılamaz. Admin panelini inceleyebilirsiniz ama kayıtlar değiştirilmez.";
            }
            context.Result = new RedirectResult(SafeReturnUrl(request));
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        // Geldiği sayfaya dön; aynı siteden değilse istatistik sayfasına
        private static string SafeReturnUrl(HttpRequest request)
        {
            var referer = request.Headers.Referer.ToString();
            if (Uri.TryCreate(referer, UriKind.Absolute, out var uri) &&
                string.Equals(uri.Host, request.Host.Host, StringComparison.OrdinalIgnoreCase) &&
                !WriteActions.IsMatch(uri.Segments.LastOrDefault()?.Trim('/') ?? string.Empty))
            {
                return uri.PathAndQuery;
            }
            return "/Admin/Statistic/Index";
        }
    }
}
