using DietBot.CosmosDB;
using DietBot.Diets.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietBot.Diets.Repository;

public class DietCosmosRepository : IDietRepository
{
    private readonly Container _container;

    public DietCosmosRepository(CosmosClient cosmosClient, IOptions<CosmosDbOptions> cosmosDbOptions)
    {
        _container = cosmosClient.GetContainer(
            cosmosDbOptions.Value.DatabaseId,
            cosmosDbOptions.Value.ContainerId);
    }

    public async Task<IEnumerable<IngredientModel>> GetDietIngredients(DietType dietType)
    {
        var queryable = _container.GetItemLinqQueryable<DietModel>();

        var conditionQuery = queryable
            .Where(d => d.Type.ToString().ToLower() == dietType.ToString().ToLower());

        using var linqFeed = conditionQuery.ToFeedIterator();

        var ingredients = new List<IngredientModel>();

        // Iterate query result pages
        while (linqFeed.HasMoreResults)
        {
            var response = await linqFeed.ReadNextAsync();

            // Iterate query results
            foreach (var diet in response)
            {
                ingredients.AddRange(diet.Ingredients);
            }
        }

        return ingredients;
    }
}
