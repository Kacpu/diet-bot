using DietBot.Diets.Models;
using System.Threading.Tasks;

namespace DietBot.Diets.Service;

public interface IDietService
{
    Task<string> AnalyzeFood(DietType dietType, string foodLabel);
}
