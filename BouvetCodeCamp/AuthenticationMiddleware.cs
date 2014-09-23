using System.Collections.Generic;
using System.Threading.Tasks;

namespace BouvetCodeCamp
{
    using System;
    using System.Security.Claims;
    using System.Text;

    using Microsoft.Owin;

    public class AuthenticationMiddleware : OwinMiddleware
    {
        public AuthenticationMiddleware(OwinMiddleware next) 
            : base(next)
        {
        }
 
        public override async Task Invoke(IOwinContext context)
        {
            context.Response.OnSendingHeaders(
                state =>
                    {
                        var resp = (OwinResponse)state;

                        if (resp.StatusCode == 401)
                            resp.Headers.Append("WWW-Authenticate", "Basic");

                    }, context.Response);

            var header = context.Request.Headers["Authorization"];

            if (!String.IsNullOrWhiteSpace(header))
            {
                var authHeader = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

                if ("Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    string parameter = Encoding.UTF8.GetString(
                        Convert.FromBase64String(authHeader.Parameter));

                    var parts = parameter.Split(':');

                    string userName = parts[0];
                    string password = parts[1];

                    if (userName == "bouvet" && password == "mysecret")
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.Name, "BBR-admindude")
                        };

                        var identity = new ClaimsIdentity(claims, "Basic");

                        context.Request.User = new ClaimsPrincipal(identity);
                    }
                }
            }
 
            await Next.Invoke(context);
        }
    }
}