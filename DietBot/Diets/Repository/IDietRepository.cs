using System.Threading.Tasks;
using DietBot.Diets.Models;

namespace DietBot.Diets.Repository;

public interface IDietRepository
{
    Task GetDiet(DietType dietType, string[] ingredients);
}
