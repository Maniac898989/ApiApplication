using ApiApplication.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
    
        public LoginController(IUserService userService)
        {
            _userService = userService;
           
        }

        [HttpPost("get-token")]
        public IActionResult Login(string UserID)
        {
            var token = _userService.CheckUser(UserID);
            if (token != null)
            {
                return Ok(token);
            }
            return BadRequest();
        }

        

    }
}
