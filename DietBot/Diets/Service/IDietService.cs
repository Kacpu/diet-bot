using DietBot.Diets.Models;
using System.Threading.Tasks;

namespace DietBot.Diets.Service;

public interface IDietService
{
    bool IsFoodLabel(string foodLabel);
    Task<string> AnalyzeFood(DietType dietType, string foodLabel);
}
