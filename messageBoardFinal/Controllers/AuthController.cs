using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using messageboardBackend.Services;
using MessageBoardBackend.Models;
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
                IdentityModelEventSource.ShowPII = true;

                var claims = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                });
                const string securityKeyString = "KoHzAdIhOsSeIn is My secret Key";
//                const string securityKeyString = "12345";
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKeyString));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var token = jwtTokenHandler.CreateJwtSecurityToken(subject: claims,
                    signingCredentials: signingCredentials);

                var tokenString = jwtTokenHandler.WriteToken(token);

                return new JwtPacket {Token = tokenString, FirstName = user.FirstName};
        }
    }
}