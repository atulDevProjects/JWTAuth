using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BakeryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class JwtAuthController : ControllerBase
    {
        private IConfiguration _config;

        public JwtAuthController(IConfiguration config)
        {
            _config = config;
        }

        public class UserModel
        {
            public string Username { get; set; }
            public string EmailAddress { get; set; }
            // public DateTime DateOfJoing { get; set; }
        }

        [HttpPost]
        public IActionResult Login([FromBody]UserModel login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { Bearer = tokenString });
                //response =  Ok(tokenString);
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                                 new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                                 new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                                 //new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
                                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                               };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                             _config["Jwt:Issuer"],
                                             claims,
                                             expires: DateTime.Now.AddSeconds(30),
                                             signingCredentials: credentials
                                             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            //Validate the User Credentials 
            //Demo Purpose, I have Passed HardCoded User Information 
            if (login.Username == "atul")
            {
                user = new UserModel { Username = "Atul kumar", EmailAddress = "test.btest@gmail.com" };
            }
            return user;
        }
    }
}