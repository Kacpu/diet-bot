using DietBot.Diets.Models;
using DietBot.Diets.Repository;
using System;
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

    public async Task<bool> IsFoodValid(DietType dietType, string foodLabel)
    {
        var ingredients = await _dietRepository.GetDietIngredients(dietType);
        return true;
    }

    private List<string> ParseIngredients(string foodLabel)
    {
        return new List<string>();
    }
}
