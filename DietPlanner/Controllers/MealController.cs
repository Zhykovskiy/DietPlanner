using DietPlanner.DAL.Entities;
using DietPlanner.Models;
using DietPlanner.Models.Meal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860




namespace DietPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<MealController> _logger;

        public MealController(IHttpClientFactory httpClientFactory, ILogger<MealController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet]
        [Route("Generate")]
        public async Task<IActionResult> Generate(double calories)
        {
            try
            {
                using var client = new HttpClient();
                //var client = _httpClientFactory.CreateClient();
                var apiKey = "9d626a8dc74647f99e0d9833af770c8f"; // Replace with your actual API key
                var url = @$"https://api.spoonacular.com/mealplanner/generate?apiKey={apiKey}&timeFrame=day&targetCalories={calories}"; // Add any other required parameters as per the API documentation
                var content = await client.GetStringAsync(url);

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();


                    DailyMealGetViewModel result = JsonConvert.DeserializeObject<DailyMealGetViewModel>(json);

                    var a = result.Recepies[0].Calories;

                    var resultOld = JsonConvert.DeserializeObject(json);

                    return Ok(a);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Failed to fetch data from API. Status code: {response.StatusCode}. Error response: {errorResponse}");
                    return StatusCode((int)response.StatusCode, "Failed to fetch data from API.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
