using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Text;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(YourProject.Startup))]

namespace YourProject
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure your API to challenge requests using JWT Bearer authentication
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode =Microsoft.Owin.Security.AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false, // Validates expiration dates on incoming JWT access tokens
                        ValidIssuer = "http://localhost",
                        ValidAudience = "http://localhost",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("gfr6dedrftyfgyuhgyugyg7f56e4sr5dtfguygyugtf"))
                    }
                });
        }
    }
}
