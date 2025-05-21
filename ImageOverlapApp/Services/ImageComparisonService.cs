using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageOverlapApp.Services
{
	public class ImageComparisonService : IImageComparisonService
	{
		private ILogger<ImageComparisonService> Logger { get; set; }
		private IConfiguration Config { get; set; }
		private IPathService PathService { get; set; }

		public ImageComparisonService(ILogger<ImageComparisonService> logger, IConfiguration config, IPathService pathService)
		{
			Logger = logger;
			Config = config;
			PathService = pathService;
		}

		public IEnumerable<(string A, string B)> CompareImages(string instanceId)
		{
			var groupA = PathService.GetGroupPath("groupA", instanceId);
			var groupB = PathService.GetGroupPath("groupB", instanceId);

			if (!Directory.Exists(groupA) || !Directory.Exists(groupB))
			{
				return null;
			}

			// Simulação de comparação simplificada
			var groupAFiles = Directory.GetFiles(groupA);
			var groupBFiles = Directory.GetFiles(groupB);

			Logger.LogInformation("Comparando {0} com {1}", groupA, groupB);

			var result = new List<(string A, string B)>();

			foreach (var fileA in groupAFiles)
			{
				foreach (var fileB in groupBFiles)
				{
					if (Path.GetFileNameWithoutExtension(fileA)[..3] ==
						Path.GetFileNameWithoutExtension(fileB)[..3])
					{
						result.Add((Path.GetFileName(fileA), Path.GetFileName(fileB)));
					}
				}
			}

			return result;
		}
	}
}