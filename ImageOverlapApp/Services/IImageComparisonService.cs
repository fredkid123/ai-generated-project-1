namespace ImageOverlapApp.Services
{
	public interface IImageComparisonService
	{
		IEnumerable<object> CompareGroups(string groupA, string groupB);
	}
}