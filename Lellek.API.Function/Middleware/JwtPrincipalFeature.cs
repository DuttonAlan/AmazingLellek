using System.Security.Claims;

namespace PlanB.DPF.Manager.Function.Middleware;

/// <summary>
/// Holds the authenticated user principal
/// for the request along with the
/// access token they used.
/// </summary>
public class JwtPrincipalFeature
{
    public JwtPrincipalFeature(ClaimsPrincipal principal, string accessToken)
    {
        Principal = principal;
        AccessToken = accessToken;
    }

    public ClaimsPrincipal Principal { get; }

    /// <summary>
    /// The access token that was used for this
    /// request. Can be used to acquire further
    /// access tokens with the on-behalf-of flow.
    /// </summary>
    public string AccessToken { get; }

    /// <summary>
    /// Users unique identifier
    /// </summary>
    public string UserIdentifier
    {
        get
        {
            return Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}