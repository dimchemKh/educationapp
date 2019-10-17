using EducationApp.BusinessLayer.Models.Auth;
using EducationApp.BusinessLayer.Common.Constants;
using EducationApp.PresentationLayer.Common;
using EducationApp.PresentationLayer.Helper.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EducationApp.PresentationLayer.Helper
{
    public class JwtHelper : IJwtHelper
    {        
        private List<Claim> GetAccessTokenClaims(AuthModel authModel)
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
        private List<Claim> GetRefreshTokenClaims(AuthModel authModel)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, authModel.UserId.ToString()),
            };
            return claims;
        }
        private string Generate(List<Claim> claims, IOptions<Config> configOptions, TimeSpan tokenExpiration)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configOptions.Value.JwtKey));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
             issuer: configOptions.Value.JwtIssuer,
             audience: configOptions.Value.JwtAudience,
             claims: claims,
             expires: DateTime.Now.Add(tokenExpiration),
             signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public AuthModel ValidateData(string token)
        {
            var authModel = new AuthModel();
            var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            if (refreshToken.ValidTo < DateTime.Now)
            {
                authModel.Errors.Add(Constants.Errors.TokenExpire);
                return authModel;
            }
            var value = refreshToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if(!long.TryParse(value, out long userId))
            {
                authModel.Errors.Add(Constants.Errors.InvalidUserId);
                return authModel;
            }
            authModel.UserId = userId;
            return authModel;
        }
        public AuthModel Generate(AuthModel authModel, IOptions<Config> configOptions)
        {
            var accessClaims = GetAccessTokenClaims(authModel);
            var refreshClaims = GetRefreshTokenClaims(authModel);
            authModel.AccessToken = Generate(accessClaims, configOptions, configOptions.Value.AccessTokenExpiration);

            authModel.RefreshToken = Generate(refreshClaims, configOptions, configOptions.Value.RefreshTokenExpiration);

            return authModel;
        }
    }
}
