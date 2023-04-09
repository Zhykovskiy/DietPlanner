using DietPlanner.DAL.Entities;
using DietPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DietPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MealController> _logger;

        public AppUserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, ILogger<MealController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> CreateAppUser(AppUserCreateViewModel user)
        {
            _logger.LogError("HELLO CreateAppUser");
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var apiKey = _configuration["apiKey"];
            var url = @$"https://api.spoonacular.com/users/connect?apiKey={apiKey}";
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseResult = JsonConvert.DeserializeObject<ConnectUserResponse>(responseContent);

            var appUser = new AppUser()
            {
                UserName = user.UserName,
                Email = user.Email,
                SpoonacularUserName = responseResult.Username,
                Hash = responseResult.Hash
            };

            try
            {
                var result = await _userManager.CreateAsync(appUser, user.Password);

                _logger.LogError("END CreateAppUser");
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(AppUserLoginViewModel model)
        {
            _logger.LogError("HELLO Login");
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ApplicationSettings:JWT_Secret")), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescription);
                var token = tokenHandler.WriteToken(securityToken);

                _logger.LogError("END Login");
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }
    }
}
