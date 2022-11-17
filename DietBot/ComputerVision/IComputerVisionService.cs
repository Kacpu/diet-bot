using System.Threading;
using System.Threading.Tasks;

namespace DietBot.ComputerVision;

public interface IComputerVisionService
{
    Task<string> ExtractText(string imageDownloadUrl, CancellationToken cancellationToken = default);
}
