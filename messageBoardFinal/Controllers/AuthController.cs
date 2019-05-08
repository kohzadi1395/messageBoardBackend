using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessageBoardBackend.Models;
using MessageBoardBackend.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace MessageBoardBackend.Controllers
{
    public class JwtPacket
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
    }


    [Produces("application/json")]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly UserRepository userRepository;

        public AuthController()
        {
            userRepository = new UserRepository();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] User user)
        {
            var login = userRepository.Login(user);

            if (login == null)
                return NotFound("email or password incorrect");

            return Ok(CreateJwtPacket(login));
        }

        [DisableCors]
        [HttpPost("register")]
        public ActionResult Register([FromBody] User user)
        {
            if (userRepository.GetUserByEmail(user.Email) != null)
                return BadRequest("User Already Exists");

            userRepository.Insert(user);
            return Ok(CreateJwtPacket(user));
        }

        private JwtPacket CreateJwtPacket(User user)
        {
                var jwtTokenHandler = new JwtSecurityTokenHandler();

                var claims = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                });
                const string securityKeyString = "KoHzAdIhOsSeIn is My secret Key";
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKeyString));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = jwtTokenHandler.CreateJwtSecurityToken(subject: claims,
                    signingCredentials: signingCredentials);

                var tokenString = jwtTokenHandler.WriteToken(token);

                return new JwtPacket {Token = tokenString, FirstName = user.FirstName};
        }

//        private JwtPacket CreateJwtPacketTest(User user)
//        {
//            IdentityModelEventSource.ShowPII = true;
//            var secretKey = Encoding.UTF8.GetBytes("KoHzAdIhOsSeIn is My secret Key"); // must be 16 character or longer
//            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);
//
//            var encryptionkey = Encoding.UTF8.GetBytes("16CharEncryptKey"); //must be 16 character
//            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
//
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.Email), //user.UserName
//                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()), //user.Id
//            };
//
//            var descriptor = new SecurityTokenDescriptor
//            {
////                Issuer = _siteSetting.JwtSettings.Issuer,
////                Audience = _siteSetting.JwtSettings.Audience,
//                IssuedAt = DateTime.Now,
//                NotBefore = DateTime.Now.AddMinutes(5),
//                Expires = DateTime.Now.AddMinutes(10),
//                SigningCredentials = signingCredentials,
//                EncryptingCredentials = encryptingCredentials,
//                Subject = new ClaimsIdentity(claims)
//            };
//
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var securityToken = tokenHandler.CreateToken(descriptor);
//            string encryptedJwt = tokenHandler.WriteToken(securityToken);
//            return new JwtPacket { Token = encryptedJwt, FirstName = user.FirstName };
//        }
    }
}