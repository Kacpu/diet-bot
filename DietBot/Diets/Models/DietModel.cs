using Newtonsoft.Json;

namespace DietBot.Diets.Models;

public class DietModel
{
    public DietType Type { get; set; }

    public IngredientModel[] Ingredients { get; set; }
}
