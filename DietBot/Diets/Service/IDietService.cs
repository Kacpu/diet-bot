using DietBot.Diets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DietBot.Diets.Service;

public interface IDietService
{
    Task GetDiet(DietType dietType, string[] ingredients);
    List<string> ParseIngredients(string text);
}
