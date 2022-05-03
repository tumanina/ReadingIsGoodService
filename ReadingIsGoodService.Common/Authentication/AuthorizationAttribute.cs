using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using ReadingIsGoodService.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Common.Authentication
{
    public class AuthorizationAttribute : TypeFilterAttribute
    {
        public AuthorizationAttribute() : base(typeof(AuthorizationFilter))
        {
        }

        public AuthorizationAttribute(string roles) : base(typeof(AuthorizationFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (HasAllowAnonymous(context.Filters))
            {
                return;
            }

            try
            {
                var user = context.HttpContext?.User;
                if (!user.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var userId = context.HttpContext.GetCurrentUserId();
                //check roles of user (by context.HttpContext.GetCurrentUserId()) if necessary
            }
            catch (Exception ex)
            {
                //todo: log exception
                context.Result = new ForbidResult();
            }

            return;
        }

        protected bool HasAllowAnonymous(IList<IFilterMetadata> filters)
        {
            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i] is IAllowAnonymousFilter)
                {
                    return true;
                }
            }

            return false;
        }
    }

}