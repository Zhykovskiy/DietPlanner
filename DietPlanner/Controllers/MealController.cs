using AutoMapper;
using DietPlanner.DAL.Entities;
using DietPlanner.Models.Meal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace DietPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<MealController> _logger;
        private readonly string _apiKey;

        public MealController(UserManager<AppUser> userManager, IConfiguration configuration, IMapper mapper, ILogger<MealController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _apiKey = _configuration["apiKey"];
        }

        [HttpGet]
        [Route("GenerateMealPlan")]
        public async Task<IActionResult> GenerateMealPlan(double targetCalories)
        {
            _logger.LogError("HELLO GenerateMealPlan");
            try
            {
                using var client = new HttpClient();
                
                var mealUrl = @$"https://api.spoonacular.com/mealplanner/generate?apiKey={_apiKey}&timeFrame=day&targetCalories={targetCalories}";
               
                var mealContent = await client.GetStringAsync(mealUrl);
                var result = JsonConvert.DeserializeObject<DailyMeal>(mealContent);

                foreach (var item in result.Meals)
                {
                    var recipeUrl = $@"https://api.spoonacular.com/recipes/{item.Id}/information?includeNutrition=true&apiKey={_apiKey}";
                    var recipeContent = await client.GetStringAsync(recipeUrl);
                    var recipeFullInfo = JsonConvert.DeserializeObject<RecipeGetViewModel>(recipeContent);

                    item.ImageUrl = recipeFullInfo.Image;
                }
                _logger.LogError("End GenerateMealPlan");
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("AddMealToPlan")]
        public async Task<IActionResult> AddMealToPlan(List<AddMealToPlanViewModel> model)
        {
            _logger.LogError("HELLO AddMealToPlan");
            var request = model.Select(item => _mapper.Map<AddToMealPlanRequest>(item)).ToList();

            var json = JsonConvert.SerializeObject(request);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var currentUser = await _userManager.GetUserAsync(User);

            var url = $@"https://api.spoonacular.com/mealplanner/{currentUser.SpoonacularUserName}/items?hash={currentUser.Hash}&apiKey={_apiKey}";
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);
            _logger.LogError("End AddMealToPlan");
            return Ok(response);
        }
    }
}
