using ImageOverlapApp.Models;

namespace ImageOverlapApp.Services
{
	public interface IImageComparisonService
	{
		IEnumerable<ComparisonResult> CompareImages(string pathA, string pathB);
	}
}
