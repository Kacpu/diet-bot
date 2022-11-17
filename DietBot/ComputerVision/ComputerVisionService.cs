using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DietBot.ComputerVision;

public class ComputerVisionService : IComputerVisionService
{
    private readonly ComputerVisionOptions _options;

    public ComputerVisionService(IOptions<ComputerVisionOptions> options)
    {
        _options = options.Value;
    }

    public async Task<string> ExtractText(string imageDownloadUrl, CancellationToken cancellationToken = default)
    {
        var client = Authenticate(_options.Endpoint, _options.Key);

        using var httpCLient = new HttpClient();
        var response = await httpCLient.GetAsync(imageDownloadUrl, cancellationToken);

        using var imageStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var textHeaders = await client.ReadInStreamAsync(imageStream, cancellationToken: cancellationToken);

        string operationLocation = textHeaders.OperationLocation;

        // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
        // We only need the ID and not the full URL
        const int numberOfCharsInOperationId = 36;
        string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

        // Extract the text
        ReadOperationResult results;

        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status == OperationStatusCodes.Running ||
            results.Status == OperationStatusCodes.NotStarted);

        var textUrlFileResults = results.AnalyzeResult.ReadResults;

        var sb = new StringBuilder();

        foreach (ReadResult page in textUrlFileResults)
        {
            foreach (Line line in page.Lines)
            {
                sb.AppendLine(line.Text);
            }
        }

        return sb.ToString();
    }

    private static ComputerVisionClient Authenticate(string endpoint, string key)
    {
        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
        {
            Endpoint = endpoint
        };

        return client;
    }
}
