using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace webapi01.Controllers
{
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpPost("~/signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult<string> SignIn(LoginViewModel login)
        {
            // 以下變數值應該透過 IConfiguration 取得
            var issuer = "JwtAuthDemo";
            var signKey = "1234567890123456"; // 請換成至少 16 字元以上的安全亂碼
            var expires = 30; // 單位: 分鐘

            if (ValidateUser(login))
            {
                return JwtHelpers.GenerateToken(issuer, signKey, login.Username, expires);
            }
            else
            {
                return BadRequest();
            }
        }

        private bool ValidateUser(LoginViewModel login)
        {
            return true; // TODO
        }

        [Authorize]
        [HttpGet("~/claims")]
        public IActionResult GetClaims()
        {
            System.Diagnostics.Debug.WriteLine("");

            return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
        }

        [Authorize]
        [HttpGet("~/username")]
        public IActionResult GetUserName()
        {         
            return Ok(User.Identity.Name);
        }


        [Authorize]
        [HttpGet("~/jwtid")]
        public IActionResult GetUniqueId()
        {       
            var jti = User.Claims.FirstOrDefault(p => p.Type == "jti");
            return Ok(jti.Value);
        }

    }
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}