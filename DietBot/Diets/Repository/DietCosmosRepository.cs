using DietBot.CosmosDB;
using DietBot.Diets.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
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

    public async Task GetDiet(DietType dietType, string[] ingredients)
    {
        IOrderedQueryable<DietModel> queryable = _container.GetItemLinqQueryable<DietModel>();

        var matches = queryable
            .Where(d => d.Type.ToString().ToLower() == dietType.ToString().ToLower());
            //.Select(d => new { d.Ingredients })
            //.Where(d => d.)
            //.Where(d => d.Ingredients.Except(ingredients));

        using FeedIterator<DietModel> linqFeed = matches.ToFeedIterator();

        // Iterate query result pages
        while (linqFeed.HasMoreResults)
        {
            FeedResponse<DietModel> response = await linqFeed.ReadNextAsync();

            // Iterate query results
            foreach (DietModel item in response)
            {
                var a = item;
            }
        }
    }
}
