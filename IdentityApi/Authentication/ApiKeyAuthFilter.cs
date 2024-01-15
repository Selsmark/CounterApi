using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Authentication
{
    public class ApiKeyAuthFilter : Attribute, IAuthorizationFilter
    {

        private readonly IConfiguration _configuration;

        public ApiKeyAuthFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out
                    var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key missing");
                return;
            }

            //var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Invalid API Key");
                return;
            }
        }
    }
}
