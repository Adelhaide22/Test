using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Test.Models;

namespace Test.Services
{
    public class IdentityService : IIdentityService
    {
        public ClaimsIdentity GetIdentity(string username, string password)
        {
            if (username != "admin" || password != "admin_password") 
                return null;
            
            var claims = new List<Claim>
            {
                new(ClaimsIdentity.DefaultNameClaimType, username),
                new(ClaimsIdentity.DefaultRoleClaimType, password)
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public string GetToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            
            var jwt = new JwtSecurityToken(
                issuer: AuthenticationOptions.ISSUER,
                audience: AuthenticationOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthenticationOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}