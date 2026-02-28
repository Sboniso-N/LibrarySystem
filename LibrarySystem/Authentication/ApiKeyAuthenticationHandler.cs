// Authentication/ApiKeyAuthenticationHandler.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using LibrarySystem.Services;

namespace LibrarySystem.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IApiKeyService apiKeyService)
            : base(options, logger, encoder, clock)
        {
            _apiKeyService = apiKeyService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-API-Key", out var apiKeyHeaderValues))
            {
                return AuthenticateResult.Fail("API Key missing");
            }

            var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(providedApiKey))
            {
                return AuthenticateResult.Fail("Invalid API Key");
            }

            var apiKey = await _apiKeyService.ValidateApiKeyAsync(providedApiKey);

            if (apiKey == null || !apiKey.IsActive)
            {
                return AuthenticateResult.Fail("Invalid API Key");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, apiKey.OwnerName)
            };

            foreach (var role in apiKey.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
}