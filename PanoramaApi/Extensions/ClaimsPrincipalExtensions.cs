using System.Security.Claims;

namespace PanoramaApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Claim GetClaim(this ClaimsPrincipal principal, string claimType)
        {
            var claim = principal.FindFirst(claimType);
            
            if (claim == null)
            {
                throw new Exception($"Claim of type \"{claimType}\" not found.");
            }

            return claim;
        }

        public static int GetId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                throw new Exception($"Claim of type \"{ClaimTypes.NameIdentifier}\" not found.");
            }

            return int.Parse(claim.Value);
        }
    }
}
