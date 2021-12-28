using BlazorDemo.Shared;
using Kenova.Server.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace BlazorDemo.Server.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("api/user/login")]
        public LoginResult LogIn(LoginCredentials credentials)
        {
            bool valid = ValidateCredentials(credentials);
            var loginResult = new LoginResult();

            if (valid == false)
            {
                loginResult.Message = "Invalid credentials.";
                return loginResult;
            }

            if (!string.Equals(credentials.UserName, "demo", StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(credentials.Password, "demo", StringComparison.OrdinalIgnoreCase))
            {
                loginResult.Message = $"User/password combination unknown for user {credentials.UserName}.";
                return loginResult;
            }

            var claims = new List<Claim>();

            claims.Add(new Claim(ServerJwtHelper.CLAIMTYPE_NAME, credentials.UserName));
            claims.Add(new Claim(ServerJwtHelper.CLAIMTYPE_ROLE, "Administrator"));

            loginResult.Authorized = true;
            loginResult.Token = ServerJwtHelper.GenerateJWT(_configuration, claims);
            loginResult.DisplayName = "Frank Th. van de Ven";

            return loginResult;
        }


        private bool ValidateCredentials(LoginCredentials credentials)
        {
            return credentials.Password?.Length > 3; // TODO: connect to some underlying store
        }

        [HttpPost("api/user/refreshtoken")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            var token = request.Token;

            var loginResult = new LoginResult();

            // validate the token crc ...
            var validated = ServerJwtHelper.ValidateCurrentToken(_configuration, token);

            if (!validated)
                throw new Exception("Invalid token.");

            // extract the username...
            var req_claims = ServerJwtHelper.ParseClaimsFromJwt(token);

            var username_claim = req_claims.Find(p => p.Type == ServerJwtHelper.CLAIMTYPE_NAME);

            if (username_claim == null)
                throw new System.Exception("Unable to extract Name from authentication token");

            var username = username_claim.Value;

            var claims = new List<Claim>();
            claims.Add(new Claim(ServerJwtHelper.CLAIMTYPE_NAME, username));
            claims.Add(new Claim(ServerJwtHelper.CLAIMTYPE_ROLE, "Administrator"));

            loginResult.Authorized = true;
            loginResult.Token = ServerJwtHelper.GenerateJWT(_configuration, claims);

            loginResult.DisplayName = "Frank Th. van de Ven";

            return Ok(loginResult);
        }


    }
}


/*
        [HttpPost("api/user/login")]
        public LoginResult LogIn(LoginCredentials credentials)
        {
            bool valid = ValidateCredentials(credentials);
            var loginResult = new LoginResult();

            if (valid == false)
            {
                loginResult.Message = "Invalid credentials.";
                return loginResult;
            }

            if (string.Equals(credentials.UserName, "demo", StringComparison.OrdinalIgnoreCase))
            {
                MakeSureDemoUserExists();
            }

            var rs = new PriKey_knUsers_Recordset();

            rs.ExecSql(KenovaServerConfig.Connector, credentials.UserName);

            if (rs.RecordCount != 1)
            {
                loginResult.Message = $"User {credentials.UserName} is unknown.";
                return loginResult;
            }

            var hasher = new PasswordHasher();

            if (hasher.Check(rs.PasswordHash, credentials.Password) == false)
            {
                loginResult.Message = $"Invalid password for user {credentials.UserName}.";
                return loginResult;
            }

            rs.LastLoginUTC = DateTime.UtcNow;
            rs.LoginCount = rs.LoginCount + 1;

            rs.SaveChanges(KenovaServerConfig.Connector);

            var claims = new List<Claim>();

            claims.Add(new Claim("nam", rs.UserName));

            var roles = rs.Roles.Split(',', StringSplitOptions.TrimEntries);

            foreach (var role in roles)
            {
                claims.Add(new Claim("rol", role));
            }

            loginResult.Authorized = true;
            loginResult.Token = ServerJwtHelper.GenerateJWT(_configuration, claims);

            return loginResult;
        }

        private void MakeSureDemoUserExists()
        {
            var rs = new PriKey_knUsers_Recordset();

            rs.ExecSql(KenovaServerConfig.Connector, "demo");

            if (rs.RecordCount == 1)
                return;

            var hasher = new PasswordHasher();

            DateTime now = DateTime.UtcNow;

            rs.Append();
            rs.UserName = "demo";
            rs.UniqueID = Guid.NewGuid();
            rs.FullName = "Frank Th. van de Ven";
            rs.EmailAddress = "fvv@sysdev.nl";
            rs.PasswordHash = hasher.Hash("demo");
            rs.Roles = "Administrator";
            rs.CreatedUTC = now;
            rs.LastLoginUTC = now;
            rs.LoginCountResetUTC = now;
            rs.LoginCount = 0;

            rs.SaveChanges(KenovaServerConfig.Connector);

        }

        private bool ValidateCredentials(LoginCredentials credentials)
        {
            return credentials.Password?.Length > 3; // TODO: connect to some underlying store
        }

 */