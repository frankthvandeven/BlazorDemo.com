// https://github.com/RickStrahl/Westwind.AspNetCore/blob/master/Westwind.AspNetCore/Security/JwtHelper.cs

// https://weblog.west-wind.com/posts/2021/Mar/09/Role-based-JWT-Tokens-in-ASPNET-Core


using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Kenova.Server.Util
{

    /*
     * https://dotnetcoretutorials.com/2020/01/15/creating-and-validating-jwt-tokens-in-asp-net-core/
     *
     */

    public static class ServerJwtHelper
    {
        public static readonly string CLAIMTYPE_NAME = "nam";
        public static readonly string CLAIMTYPE_ROLE = "rol";


        public static string GenerateJWT(IConfiguration configuration, List<Claim> claims)
        {
            // Also consider using AsymmetricSecurityKey if you want the client to be able to validate the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"],
                                             audience: configuration["Jwt:Audience"],
                                             claims: claims,
                                             notBefore: null,
                                             expires: DateTime.UtcNow.AddMinutes(120),
                                             signingCredentials: signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            string signature = tokenHandler.WriteToken(token);

            return signature;
        }

        public static bool ValidateCurrentToken(IConfiguration configuration, string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }





        // Copied from Client code
        public static List<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var list = new List<Claim>();

            foreach (var kvp in keyValuePairs)
                list.Add(new Claim(kvp.Key, kvp.Value.ToString()));

            return list;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }


    }
}