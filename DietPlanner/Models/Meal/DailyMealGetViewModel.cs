namespace DietPlanner.Models.Meal;

public class DailyMealGetViewModel
{
    public List<Recipe> Meals { get; set; }
    public Nutrients Nutrients { get; set; }
}