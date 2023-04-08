namespace DietPlanner.Models.Meal;

public class DailyMeal
{
    public List<Recipe> Meals { get; set; }
    public Nutrients Nutrients { get; set; }
}