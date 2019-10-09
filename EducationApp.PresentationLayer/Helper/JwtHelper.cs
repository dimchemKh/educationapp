using EducationApp.BusinessLayer.Models.Auth;
using EducationApp.DataAccessLayer.Common.Constants;
using EducationApp.PresentationLayer.Common;
using EducationApp.PresentationLayer.Helper.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EducationApp.PresentationLayer.Helper
{
    public class JwtHelper : IJwtHelper
    {
        private List<Claim> GetAccessClaims(AuthDetailsModel authModel)
        {
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, authModel.UserId.ToString()),
                new Claim(ClaimTypes.Role, authModel.UserRole),
                new Claim(ClaimTypes.Name, authModel.UserName)
            };
            return claims;
        }
        private List<Claim> GetRefreshClaims(AuthDetailsModel authModel)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, authModel.UserId.ToString()),
            };
            return claims;
        }
        private string GetToken(List<Claim> claims, IOptions<Config> configOptions, TimeSpan tokenExpiration)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configOptions.Value.JwtKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer: configOptions.Value.JwtIssuer,
             audience: "WebApiEducationApp",
             claims: claims,
             expires: DateTime.Now.Add(tokenExpiration),
             signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public AuthModel CheckAccess(string token)
        {
            var authModel = new AuthDetailsModel();
            var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            if (refreshToken.ValidTo < DateTime.Now)
            {
                authModel.Errors.Add(Constants.Errors.TokenExpire);
                return authModel;
            }
            var value = refreshToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            authModel.UserId = long.Parse(value);
            return authModel;
        }
        public AuthModel Generate(AuthModel authModel, IOptions<Config> configOptions)
        {
            var accessClaims = GetAccessClaims(authModel as AuthDetailsModel);
            var refreshClaims = GetRefreshClaims(authModel as AuthDetailsModel);
            authModel.AccessToken = GetToken(accessClaims, configOptions, configOptions.Value.AccessTokenExpiration);
            authModel.RefreshToken = GetToken(refreshClaims, configOptions, configOptions.Value.RefreshTokenExpiration);
            return authModel;
        }
    }
}
