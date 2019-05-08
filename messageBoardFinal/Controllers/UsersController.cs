using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MessageBoardBackend.Core;
using MessageBoardBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoardBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UserRepository userRepository;

        public UsersController()
        {
            userRepository = new UserRepository();
        }

        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            var user = userRepository.GetUser(id);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

       [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            return Ok("Secure");
        }
    }
}