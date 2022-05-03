using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ReadingIsGoodService.Common.Authentication
{
    internal class JwtTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string AuthenticationScheme = "Bearer";
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public JwtTokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue authHeader))
            {
                try
                {
                    if (string.IsNullOrEmpty(authHeader?.Parameter) || !authHeader.Scheme.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                    {
                        return AuthenticateResult.Fail("Invalid Authorization Header");
                    }

                    var jwtTokenString = authHeader.Parameter;
                    var token = Read(jwtTokenString);

                    /* full version must contain token validation based
                    var user = await _userService.GetByUsername(GetEmailFromClaims(token.Claims));
                    if (user == null)
                    {
                        return AuthenticateResult.Fail("User not found");
                    }
                    var principal = _tokenService.Validate(jwtTokenString);
                    if (principal == null)
                    {
                        return AuthenticateResult.Fail("Token validation failed");
                    }
                    var claimsIdentity = (ClaimsIdentity)principal.Identity;
                    claimsIdentity.AddClaim(new Claim(Constants.UserIdClaimName, user.Id.ToString()));*/

                    var identity = new ClaimsIdentity(token.Claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
                catch (Exception ex)
                {
                    return AuthenticateResult.Fail(ex.Message);
                }
            }
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        private JwtSecurityToken Read(string token)
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);

            if (string.IsNullOrEmpty(jwtToken.Subject))
            {
                throw new Exception("Token does not contain subject");
            }

            return jwtToken;
        }

        private string GetEmailFromClaims(IEnumerable<Claim> claims)
        {
            if (claims == null || !claims.Any())
            {
                throw new Exception("No claims found");
            }

            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email || x.Type == JwtRegisteredClaimNames.Email);

            if (email == null)
            {
                throw new Exception("Email claim not found");
            }

            return email.Value;
        }
    }
}
