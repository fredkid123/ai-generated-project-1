using System.Collections.Generic;

namespace ImageOverlapApp.Services
{
	public interface IImageComparisonService
	{
		IEnumerable<(string A, string B)> CompareImages(string groupADir, string groupBDir);
	}
}