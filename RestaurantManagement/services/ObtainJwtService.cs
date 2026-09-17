using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Services.Interface;
using Superpower.Parsers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using Claim = System.Security.Claims.Claim;


namespace RestaurantManagement.Services
{
    public class ObtainJwtService:IObtainJwtService
    {
        public string CraftJwt(User user)
        {
            DotNetEnv.Env.Load();
            string key = Environment.GetEnvironmentVariable("SECRET_KEY"); //Secret key which will be used later during validation    
            var issuer = Environment.GetEnvironmentVariable("ISSUER");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Standard User ID Claim
                new Claim(ClaimTypes.Role, user.Role.ToString())             // Standard Role Claim
            };
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: permClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        }
    }
