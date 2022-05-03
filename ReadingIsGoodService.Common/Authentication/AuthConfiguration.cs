using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;

namespace ReadingIsGoodService.Common.Authentication
{
    public static class AuthConfiguration
    {
        public const string DefaultScheme = "Smart";

        public static IServiceCollection ConfigureAuth(this IServiceCollection services)
        {
            using (var provider = services.BuildServiceProvider())
            {
                services.AddSingleton<IAsyncAuthorizationFilter, AuthorizationFilter>();
                services.AddSingleton<IAuthenticationHandler, JwtTokenAuthenticationHandler>();
                services.AddSingleton<IAuthenticationHandler, BasicAuthenticationHandler>();

                services.AddAuthentication(DefaultScheme)
                    .AddScheme<AuthenticationSchemeOptions, JwtTokenAuthenticationHandler>(JwtTokenAuthenticationHandler.AuthenticationScheme, _ => { })
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.AuthenticationScheme, _ => { })
                    .AddPolicyScheme(DefaultScheme, "Bearer or Basic Authentication", polSchemOpt =>
                    {
                        polSchemOpt.ForwardDefaultSelector = context =>
                        {
                            if (context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authValue))
                            {
                                if (authValue.First().StartsWith($"{JwtBearerDefaults.AuthenticationScheme} ", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    return JwtBearerDefaults.AuthenticationScheme;
                                }
                            }
                            return BasicAuthenticationHandler.AuthenticationScheme;
                        };
                    });
            }

            return services;
        }
    }
}
