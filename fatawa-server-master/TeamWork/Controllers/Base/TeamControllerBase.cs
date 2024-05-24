using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TeamWork.Controllers.Base
{
    [Controller]
    public class TeamControllerBase : Controller
    {
        public TeamControllerBase()
        {

        }

        public ClaimsPrincipal CurrentClaimPrinciple { get { return HttpContext.User; } }
        public Claim CurrentClaim { get { return CurrentClaimPrinciple.FindFirst(ClaimTypes.NameIdentifier); } }
        protected int CurrentUserId => CurrentClaimPrinciple != null && CurrentClaim != null ? int.Parse(CurrentClaim != null ? CurrentClaim.Value : "0") : 0;
        protected string CurrentUserRole => HttpContext.User != null ? HttpContext.User.FindFirst(ClaimTypes.Role).Value : string.Empty;
    }
}
