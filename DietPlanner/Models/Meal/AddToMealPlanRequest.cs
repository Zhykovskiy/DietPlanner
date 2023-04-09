namespace DietPlanner.Models.Meal
{
    public class AddToMealPlanRequest
    {
        public decimal Date { get; set; }
        public int? Slot { get; set; }
        public int? Position { get; set; }
        public string Type { get; set; }
        public MealPlanValue Value { get; set; }
    }
}
