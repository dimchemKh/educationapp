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
using EducationApp.BusinessLayer.Models.Users;

namespace EducationApp.PresentationLayer.Helper
{
    public class JwtHelper : IJwtHelper
    {
        public const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

        private List<Claim> GetAccessTokenClaims(UserInfoModel authModel)
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
        private List<Claim> GetRefreshTokenClaims(UserInfoModel authModel)
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
            var credential = new SigningCredentials(key, SecurityAlgorithm);

            var token = new JwtSecurityToken(
             issuer: configOptions.Value.JwtIssuer,
             audience: configOptions.Value.JwtAudience,
             claims: claims,
             expires: DateTime.Now.Add(tokenExpiration),
             signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public UserInfoModel ValidateData(string token)
        {
            var userInfoModel = new UserInfoModel();
            var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            if (refreshToken.ValidTo < DateTime.Now)
            {
                userInfoModel.Errors.Add(Constants.Errors.TokenExpire);
                return userInfoModel;
            }
            var value = refreshToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if(!long.TryParse(value, out long userId))
            {
                userInfoModel.Errors.Add(Constants.Errors.UserIdInvalid);
                return userInfoModel;
            }
            userInfoModel.UserId = userId;
            return userInfoModel;
        }
        public AuthModel Generate(UserInfoModel userInfoModel, IOptions<Config> configOptions)
        {
            var result = new AuthModel();
            var accessClaims = GetAccessTokenClaims(userInfoModel);
            var refreshClaims = GetRefreshTokenClaims(userInfoModel);
            result.AccessToken = Generate(accessClaims, configOptions, configOptions.Value.AccessTokenExpiration);

            result.RefreshToken = Generate(refreshClaims, configOptions, configOptions.Value.RefreshTokenLongExpiration);

            return result;
        }
    }
}
