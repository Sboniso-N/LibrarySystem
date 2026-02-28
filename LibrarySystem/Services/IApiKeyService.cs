// Services/IApiKeyService.cs
using LibrarySystem.Models;

namespace LibrarySystem.Services
{
    public interface IApiKeyService
    {
        Task<ApiKey?> ValidateApiKeyAsync(string apiKey);
        Task<ApiKey> GenerateApiKeyAsync(string ownerName, List<string> roles, DateTime? expiresAt = null);
        Task<bool> RevokeApiKeyAsync(string apiKey);
    }

    public class ApiKeyService : IApiKeyService
    {
        private static readonly List<ApiKey> _apiKeys = new()
        {
            new ApiKey {
                Id = 1,
                Key = "AdminKey123",
                OwnerName = "Library Admin",
                Roles = new List<string> { "Admin" },
                CreatedAt = DateTime.UtcNow
            },
            new ApiKey {
                Id = 2,
                Key = "UserKey456",
                OwnerName = "Library User",
                Roles = new List<string> { "User" },
                CreatedAt = DateTime.UtcNow
            }
        };

        public Task<ApiKey?> ValidateApiKeyAsync(string apiKey)
        {
            var key = _apiKeys.FirstOrDefault(k => k.Key == apiKey && k.IsActive);
            return Task.FromResult(key);
        }

        public Task<ApiKey> GenerateApiKeyAsync(string ownerName, List<string> roles, DateTime? expiresAt = null)
        {
            var newApiKey = new ApiKey
            {
                Id = _apiKeys.Count + 1,
                Key = GenerateSecureKey(),
                OwnerName = ownerName,
                Roles = roles,
                ExpiresAt = expiresAt,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _apiKeys.Add(newApiKey);
            return Task.FromResult(newApiKey);
        }

        public Task<bool> RevokeApiKeyAsync(string apiKey)
        {
            var key = _apiKeys.FirstOrDefault(k => k.Key == apiKey);
            if (key != null)
            {
                key.IsActive = false;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        private static string GenerateSecureKey()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}