using DietBot.Diets.Models;
using System.Threading.Tasks;

namespace DietBot.Diets.Service;

public interface IDietService
{
    Task<bool> IsFoodValid(DietType dietType, string foodLabel);
}
