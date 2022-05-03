using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ReadingIsGoodService.Common.Extensions
{
    public static class HttpContextExtension
    {
        public static int GetCurrentUserId(this HttpContext context)
        {
            var user = context?.User;
            if (!user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException();
            }

            if (!int.TryParse(GetUserIdFromClaims(user.Claims), out int userId))
            {
                throw new UnauthorizedAccessException();
            }

            return userId;
        }

        private static string GetUserIdFromClaims(IEnumerable<Claim> claims)
        {
            if (claims == null || !claims.Any())
            {
                throw new Exception("No claims found");
            }

            var userId = claims.FirstOrDefault(x => x.Type == "userid");

            if (userId == null)
            {
                throw new Exception("userid claim not found");
            }

            return userId.Value;
        }
    }
}