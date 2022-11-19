using DietBot.Diets.Models;
using DietBot.Diets.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DietBot.Diets.Service;

public class DietService : IDietService
{
    private readonly IDietRepository _dietRepository;

    public DietService(IDietRepository dietRepository)
    {
        _dietRepository = dietRepository;
    }

    public async Task<string> AnalyzeFood(DietType dietType, string foodLabel)
    {
        var parsedFoodLabel = foodLabel.Substring(
            foodLabel.IndexOf("Ingredients", StringComparison.InvariantCultureIgnoreCase) + "Ingredients".Length);
        var checkIngredients = parsedFoodLabel.Split(',');

        var ingredients = await _dietRepository.GetDietIngredients(dietType);

        var validIngredientsCount = 0;

        foreach (var checkIngredient in checkIngredients)
        {
            if (ingredients.Any(i => i.Name.ToLower().Contains(checkIngredient.ToLower())))
            {
                validIngredientsCount++;
            }
        }

        double value = (double)validIngredientsCount / checkIngredients.Length;

        return value switch
        {
            < 0.25 => "This food is not good for chosen diet. 👎",
            < 0.50 => "This food is doubtful for chosen diet. 🤔",
            < 0.75 => "This food should be good for chosen diet. 👍",
            _ => "This food is good for your diet. 😍",
        };
    }
}
