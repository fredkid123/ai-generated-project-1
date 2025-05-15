public interface IImageComparisonService
{
	List<object> Compare(string groupAPath, string groupBPath);
}

public class ImageComparisonService : IImageComparisonService
{
	public List<object> Compare(string groupAPath, string groupBPath)
	{
		var filesA = Directory.GetFiles(groupAPath);
		var filesB = Directory.GetFiles(groupBPath);

		var results = filesA.SelectMany(a => filesB, (a, b) =>
		{
			var nameA = Path.GetFileNameWithoutExtension(a);
			var nameB = Path.GetFileNameWithoutExtension(b);

			var overlap = nameA.Length >= 3 && nameB.Length >= 3 && nameA[..3] == nameB[..3];

			if (overlap)
			{
				return new
				{
					A = Path.GetFileName(a),
					B = Path.GetFileName(b),
					Overlap = true
				};
			}
			else
			{
				return null;
			}
		})
		.Where(x => x != null)
		.ToList<object>();

		return results;
	}
}