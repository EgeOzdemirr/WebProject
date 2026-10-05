using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace WebProject.WebUI.Conventions
{
    public class AreaAuthorizationConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (!controller.RouteValues.TryGetValue("area", out var area))
            {
                return;
            }

            if (string.Equals(area, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                controller.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole("Admin", DemoReadOnlyFilter.Role)
                    .Build()));
                controller.Filters.Add(new DemoReadOnlyFilter());
            }
            else if (string.Equals(area, "AppUser", StringComparison.OrdinalIgnoreCase))
            {
                controller.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build()));
            }
        }
    }
}
