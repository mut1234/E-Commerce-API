using E_Commerce_API.Dto.Auth;
using E_Commerce_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Commerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ECommerceDbContext _eCommerceDbContext;
        private IConfiguration _config;
        public AuthController(ILogger<AuthController> logger, ECommerceDbContext eCommerceDbContext ,IConfiguration configuration)
        {
            _eCommerceDbContext = eCommerceDbContext;
            _logger = logger;
            _config = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = _eCommerceDbContext.Users
                    .FirstOrDefault(u => u.Username.ToUpper() == loginDto.Username.ToUpper());

                if (user == null)
                {
                    return BadRequest("Invalid username or password");
                }
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return BadRequest("Invalid username or password");
                }
                var token = GenrateJwtToken(user);
                
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var existingUser = _eCommerceDbContext.Users
                    .FirstOrDefault(u => u.Username.ToUpper() == registerDto.Username.ToUpper());
                if (existingUser != null)
                {
                    return BadRequest("Username already exists");
                }
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
                var newUser = new User
                {
                    FullName = registerDto.FullName,
                    Username =  registerDto.Username,
                    PasswordHash = hashedPassword,
                    Email = registerDto.Email,
                    Role = "Visitor" // Default role
                };
                _eCommerceDbContext.Users.Add(newUser);
                _eCommerceDbContext.SaveChanges();
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during registration.");
                return StatusCode(500, "Internal server error");
            }
        }
        private string GenrateJwtToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name,user.Username));

            if(!string.IsNullOrEmpty(user.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role,user.Role));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
            string tokenExpiration = DateTime.Now.AddHours(2).ToString();
            string token = new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    issuer: _config["Jwt:issuer"],
                    audience : _config["Jwt:audience"],
                    signingCredentials: creds
                )
            );  
        
            return token;   

        }
    }
}
