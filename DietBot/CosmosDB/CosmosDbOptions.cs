namespace DietBot.CosmosDB;

public class CosmosDbOptions
{
    public const string Section = "CosmosDb";

    public string AuthKey { get; set; }

    public string Endpoint { get; set; }

    public string DatabaseId { get; set; }

    public string ContainerId { get; set; }
}
