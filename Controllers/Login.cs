using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Authenticate.Model;

namespace Authenticate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        private IConfiguration _config;

        public Login(IConfiguration config)
        {
            _config = config;
        }

        private List<UserModel> users = new List<UserModel>
        {
            new UserModel
            {
                Username = "caleb",
                Password = "africana"
            }
        };


        private UserModel ValidateUser (UserModel loginDetails)
        {
            var result = users.SingleOrDefault(x => x.Username == loginDetails.Username && x.Password == loginDetails.Password);

            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }            
        }

        private string GenerateWebToken()
        {
            return "some String";
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login()
        {
            
        }

    }
}
