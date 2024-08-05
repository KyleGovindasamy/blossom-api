using BlossomWebApi.DataContext;
using BlossomWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlossomWebApi.Dto;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace BlossomWebApi.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public IdentityController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, 
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _db = db;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] NewUserDto model)
        {
            {
                ApplicationUser appUser = new ApplicationUser()
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.ContactNumber
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(appUser, model.Role);
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    return Ok(appUser);
                }
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            ApplicationUser? appUser = await _userManager.FindByEmailAsync(model.Email);
            if (appUser != null)
            {
                SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                string token = CreateToken(appUser);
                return Ok(new
                { Token = token, Email = appUser.Email, FirstName = appUser.FirstName, LastName = appUser.LastName });
            }
            return BadRequest("User not found");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        private string CreateToken(ApplicationUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
