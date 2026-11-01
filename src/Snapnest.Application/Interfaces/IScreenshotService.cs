using System.Threading.Tasks;

namespace Snapnest.Application.Interfaces;

public interface IScreenshotService
{
    Task<string> CaptureFullScreenAsync();
    void DiscardTempImage(string tempFilePath);
    string GetNormalSaveDirectory();
}