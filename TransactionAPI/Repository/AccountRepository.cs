using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TransactionAPI.Dto;
using TransactionAPI.Interfaces;
using TransactionAPI.Models;
using static TransactionAPI.Dto.ServiceResponses;

namespace TransactionAPI.Repository
{
    public class AccountRepository : IUserAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<GeneralResponse> CreateAccount(UserDTO userDTO)
        {
            if (userDTO is null)
            {
                return new GeneralResponse(false, "Model is empty");
            }
            var newUser = new ApplicationUser()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                PasswordHash = userDTO.Password,
                UserName = userDTO.Email
            };
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            if (user is not null)
            {
                return new GeneralResponse(false, "User registered already");
            }

            var createUser = await _userManager.CreateAsync(newUser!, userDTO.Password);
            if (!createUser.Succeeded)
            {
                return new GeneralResponse(false, "Error occurred.. please try again");
            }
            var checkAdmin = await _roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await _userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Account Created");
            }
            else
            {
                var checkUser = await _roleManager.FindByNameAsync("User");
                if (checkUser is null)
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                }
                await _userManager.AddToRoleAsync(newUser, "User");
                return new GeneralResponse(true, "Account Created");
            }
        }

        public async Task<string> GetUserNameById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            return user?.UserName;
        }

        public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return new LoginResponse(false, null!, "Login container is empty");
            }

            var getUser = await _userManager.FindByEmailAsync(loginDTO.Email);


            if (getUser is null) 
            { 
                return new LoginResponse(false, null!, "User not found");
            }
            bool checkUserPasswords = await _userManager.CheckPasswordAsync(getUser, loginDTO.Password);

            if (!checkUserPasswords)
            {
                return new LoginResponse(false, null!, "Invalid email/password");
            }
       
            var getUserRole = await _userManager.GetRolesAsync(getUser);

            var userSession = new UserSession(getUser.Id, getUser.Name, getUser.Email, getUserRole.First());

            string token = GenerateToken(userSession);

            return new LoginResponse(true, token!, "Login completed");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
