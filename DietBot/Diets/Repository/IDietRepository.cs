using System.Collections.Generic;
using System.Threading.Tasks;
using DietBot.Diets.Models;

namespace DietBot.Diets.Repository;

public interface IDietRepository
{
    Task<IEnumerable<IngredientModel>> GetDietForbiddenIngredients(DietType dietType);
}
