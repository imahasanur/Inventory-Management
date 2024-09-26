using System.Security.Claims;

namespace InventoryManagement.Data.Membership
{
    public interface ITokenService
    {
        Task<string> GetJwtToken(IList<Claim> claims, string key, string issuer, string audience);
    }
}