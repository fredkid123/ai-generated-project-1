using ImageOverlapApp.Models;

namespace ImageOverlapApp.Services
{
	public interface IImageComparisonService
	{
		IEnumerable<ComparisonResult> CompareImages(string groupA, string groupB, string instanceId);
	}
}
