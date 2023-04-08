namespace DietPlanner.Models.Meal;

public class RecipeInfo
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string SourceUrl { get; set; }
    public string ImageUrl { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbohydrates { get; set; }
}