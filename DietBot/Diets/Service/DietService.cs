using DietBot.Diets.Models;
using DietBot.Diets.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietBot.Diets.Service;

public class DietService : IDietService
{
    private readonly IDietRepository _dietRepository;

    public DietService(IDietRepository dietRepository)
    {
        _dietRepository = dietRepository;
    }

    public async Task GetDiet(DietType dietType, string[] ingredients)
    {
        await _dietRepository.GetDiet(dietType, ingredients);
    }

    public List<string> ParseIngredients(string text)
    {
        return new List<string>();
    }
}
