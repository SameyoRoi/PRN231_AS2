using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PE_SE173338.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class LoginController : ControllerBase
    {
        private readonly IBranchAccountRepo _repo;
        public LoginController(IBranchAccountRepo repo)
        {
            _repo = repo;
        }


        [HttpPost("Login")]
        //[ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var account = await _repo.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }

            //Generate JWT Token
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.EmailAddress),
                new Claim("Role", account.Role.ToString()),
                new Claim("AccountId", account.AccountId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(preparedToken);
            var role = account.Role.ToString(); //0:Admin 1:Staff 2:Manager
            var accountId = account.AccountId.ToString();
            return Ok(new LoginResponseDTO
            {
                Role = role,
                Token = token,
                AccountId = accountId,
                 User= account,
                
            });
        }

    }
}
