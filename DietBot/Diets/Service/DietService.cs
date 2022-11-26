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

    public bool IsFoodLabel(string foodLabel)
    {
        return foodLabel.Contains("Ingredients", StringComparison.InvariantCultureIgnoreCase);
    }

    public async Task<string> AnalyzeFood(DietType dietType, string foodLabel)
    {
        var parsedFoodLabel = foodLabel[
            (foodLabel.IndexOf("Ingredients", StringComparison.InvariantCultureIgnoreCase) + "Ingredients".Length)..];

        var ingredientsToCheck = parsedFoodLabel.Split(',');

        var forbiddenIngredients = await _dietRepository.GetDietForbiddenIngredients(dietType);

        foreach (var ingredientToCheck in ingredientsToCheck)
        {
            if (forbiddenIngredients.Any(i => i.Name.ToLower().Contains(ingredientToCheck.ToLower())))
            {
                return "This food is not good for chosen diet. 👎";
            }
        }

        return "This food is good for your diet. 👍\"";
    }
}
