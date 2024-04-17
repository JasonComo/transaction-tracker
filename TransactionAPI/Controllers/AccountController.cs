using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransactionAPI.Dto;
using TransactionAPI.Interfaces;

namespace TransactionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccount _userAccount;

        public AccountController(IUserAccount userAccount)
        {
            _userAccount = userAccount;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            var response = await _userAccount.CreateAccount(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await _userAccount.LoginAccount(loginDTO);
            return Ok(response);
        }

        [HttpGet("username")]
        [Authorize]
        public async Task<IActionResult> GetUserNameById()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userName = await _userAccount.GetUserNameById(userId);

                if (userName == null)
                {
                    return NotFound();
                }

                return Ok(userName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }


        }
    }
}
