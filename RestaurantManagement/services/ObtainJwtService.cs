using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Services.Interface;
using Claim = System.Security.Claims.Claim;


namespace RestaurantManagement.Services
{
    public class ObtainJwtService:IObtainJwtService
    {
            public string CraftJwt(User user)
            {
                string key = "gfr6dedrftyfgyuhgyugyg7f56e4sr5dtfguygyugtf"; //Secret key which will be used later during validation    
                var issuer = "http://localhost";

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var permClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",$"{user.userId}"),
                new Claim("Role", $"{user.Role}")
            };
                var token = new JwtSecurityToken(issuer,
                    issuer,
                    permClaims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }


        }
    }
