using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreAPIJWT.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreAPIJWT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AuthenticateController : ControllerBase
    {
        private readonly JWTAppSettings _appSettings;
        public AuthenticateController(IOptions<JWTAppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post()
        {
            var user = new User();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "USER NAME"),
                    new Claim(ClaimTypes.DateOfBirth, "01/01/01")

                }),          
            
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return Ok(user);
        }
    }
}