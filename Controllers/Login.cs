using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Authenticate.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections;

namespace Authenticate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        private List<UserModel> users = new List<UserModel>
        {
            new UserModel
            {
                Username = "caleb",
                Email = "c.omoijuanfo@gmail.com"
            }
        };

        private UserModel ValidateUser(UserModel loginDetails)
        {
            var result = users.SingleOrDefault(x => x.Username == loginDetails.Username && x.Email == loginDetails.Email);

            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }       

        private string GenerateWebToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
               };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddSeconds(120),
                signingCredentials: credentials);

            return tokenHandler.WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(UserModel userDetails)
        {
            var user = this.ValidateUser(userDetails);

            var token = this.GenerateWebToken(user);

            return Ok(new
            {
                token
            });

        }

        [HttpGet("User")]
        public ActionResult<IEnumerable> GetUser ()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    return users;
                } 
                else
                {
                    return BadRequest(new
                    {
                        Message = "Wrong/Expired Token"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }           
        }
    }
}
