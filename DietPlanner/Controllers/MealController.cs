using DietPlanner.Models.Meal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DietPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        [HttpGet]
        [Route("GenerateMealPlan")]
        public async Task<IActionResult> GenerateMealPlan(double targetCalories)
        {
            try
            {
                using var client = new HttpClient();
                var apiKey = "031d6e7cded746119c46900569a5fb0d";
                var mealUrl = @$"https://api.spoonacular.com/mealplanner/generate?apiKey={apiKey}&timeFrame=day&targetCalories={targetCalories}";
               
                var mealContent = await client.GetStringAsync(mealUrl);
                var result = JsonConvert.DeserializeObject<DailyMealGetViewModel>(mealContent);

                foreach (var item in result.Meals)
                {
                    var recipeUrl = $@"https://api.spoonacular.com/recipes/{item.Id}/information?includeNutrition=true&apiKey={apiKey}";
                    var recipeContent = await client.GetStringAsync(recipeUrl);
                    var recipeFullInfo = JsonConvert.DeserializeObject<RecipeGetViewModel>(recipeContent);

                    item.ImageUrl = recipeFullInfo.Image;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
