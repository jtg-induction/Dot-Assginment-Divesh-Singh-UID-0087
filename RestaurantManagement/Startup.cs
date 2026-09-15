using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Configuration;
using System.Text;
using System.Web.Http;


[assembly: Microsoft.Owin.OwinStartup(typeof(RestaurantManagement.Startup))]

namespace RestaurantManagement
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            // 1. Get the actual absolute root directory of your web application
            string baseDir = System.Web.HttpRuntime.AppDomainAppPath;

            // 2. Combine it with the .env filename to create an absolute path
            string envFilePath = System.IO.Path.Combine(baseDir, ".env");

            // 3. Force DotNetEnv to load this specific file path
            DotNetEnv.Env.Load(envFilePath);
            string issuerAndAudience = Environment.GetEnvironmentVariable("ISSUER");
            string securitySecret = Environment.GetEnvironmentVariable("SECRET_KEY");
            var secretBytes = Encoding.UTF8.GetBytes(securitySecret);
            var symmetricKey = new SymmetricSecurityKey(secretBytes);
            string base64Secret = Convert.ToBase64String(secretBytes);
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    // This links it perfectly to the HostAuthenticationFilter("Bearer") in WebApiConfig!
                    AuthenticationType = "Bearer",

                    TokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler(),

                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                new SymmetricKeyIssuerSecurityKeyProvider(issuerAndAudience, base64Secret)
                    },

                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuerAndAudience,

                        ValidateAudience = true,
                        ValidAudience = issuerAndAudience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = symmetricKey,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }
                });
        }

    }
}