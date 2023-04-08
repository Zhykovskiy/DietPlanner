using DietPlanner.DAL.Entities;
using DietPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public AppUserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> CreateAppUser(AppUserCreateViewModel user)
        {
            var json = JsonConvert.SerializeObject(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://api.spoonacular.com/users/connect?apiKey=031d6e7cded746119c46900569a5fb0d";
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

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
