using EducationApp.BusinessLogic.Models.Auth;
using EducationApp.BusinessLogic.Common.Constants;
using EducationApp.Presentation.Common.Models.Configs;
using EducationApp.Presentation.Helper.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using EducationApp.BusinessLogic.Models.Users;

namespace EducationApp.Presentation.Helper
{
    public class JwtHelper : IJwtHelper
    {
        private readonly IOptions<AuthConfig> _authConfig;
        private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

        public JwtHelper(IOptions<AuthConfig> authConfig)
        {
            _authConfig = authConfig;
        }

        private List<Claim> GetAccessTokenClaims(UserInfoModel authModel)
        {
            var claims = GetRefreshTokenClaims(authModel);

            claims.Add(new Claim(ClaimTypes.Role, authModel.UserRole));
            claims.Add(new Claim(ClaimTypes.Name, authModel.UserName));

            return claims;
        }
        private List<Claim> GetRefreshTokenClaims(UserInfoModel authModel)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, authModel.UserId.ToString()));

            return claims;
        }
        private string Generate(List<Claim> claims, TimeSpan tokenExpiration)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authConfig.Value.JwtKey));

            var credential = new SigningCredentials(key, SecurityAlgorithm);

            var token = new JwtSecurityToken(
             issuer: _authConfig.Value.JwtIssuer,
             audience: _authConfig.Value.JwtAudience,
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
        public AuthModel Generate(UserInfoModel userInfoModel)
        {
            var result = new AuthModel();

            var accessClaims = GetAccessTokenClaims(userInfoModel);

            var refreshClaims = GetRefreshTokenClaims(userInfoModel);

            result.AccessToken = Generate(accessClaims, _authConfig.Value.AccessTokenExpiration);

            result.RefreshToken = Generate(refreshClaims, _authConfig.Value.RefreshTokenExpiration);

            return result;
        }
    }
}
