using Microsoft.Extensions.Logging;
using System.IO;

namespace ImageOverlapApp.Services
{
	public class ImageComparisonService : IImageComparisonService
	{
		private ILogger<ImageComparisonService> Logger { get; set; }

		public ImageComparisonService(ILogger<ImageComparisonService> logger)
		{
			Logger = logger;
		}

		public IEnumerable<object> CompareGroups(string groupA, string groupB)
		{
			string groupADir = Path.Combine("wwwroot", groupA);
			string groupBDir = Path.Combine("wwwroot", groupB);

			if (!Directory.Exists(groupADir) || !Directory.Exists(groupBDir))
			{
				Logger.LogWarning("Um dos diretórios de comparação não existe: {groupADir}, {groupBDir}", groupADir, groupBDir);
				return Enumerable.Empty<object>();
			}

			Logger.LogInformation("Comparando arquivos entre {groupA} e {groupB}", groupADir, groupBDir);

			var groupAFiles = Directory.GetFiles(groupADir).Select(Path.GetFileName);
			var groupBFiles = Directory.GetFiles(groupBDir).Select(Path.GetFileName);

			return groupAFiles.SelectMany(a => groupBFiles, (a, b) => new
			{
				A = a,
				B = b,
				Overlap = Path.GetFileNameWithoutExtension(a).Substring(0, 3)
						 == Path.GetFileNameWithoutExtension(b).Substring(0, 3)
			});
		}
	}
}