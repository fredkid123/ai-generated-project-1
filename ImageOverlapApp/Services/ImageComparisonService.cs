using ImageOverlapApp.Models;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageOverlapApp.Services
{
	public class ImageComparisonService : IImageComparisonService
	{
		private readonly ILogger<ImageComparisonService> _logger;
		private readonly float _threshold;
		private readonly IPathService _pathService;

		public ImageComparisonService(
			ILogger<ImageComparisonService> logger,
			IConfiguration config,
			IPathService pathService)
		{
			_logger = logger;
			_threshold = config.GetValue<float>("SimilarityThreshold", 0.85f);
			_pathService = pathService;
		}

		public IEnumerable<ComparisonResult> CompareImages(string groupA, string groupB, string instanceId)
		{
			_logger.LogInformation("Comparing images using SSIM with threshold {Threshold}", _threshold);

			var pathA = _pathService.GetGroupPath(groupA, instanceId);
			var pathB = _pathService.GetGroupPath(groupB, instanceId);

			if (!Directory.Exists(pathA) || !Directory.Exists(pathB))
			{
				_logger.LogWarning("One or both image groups not found.");
				return Enumerable.Empty<ComparisonResult>();
			}

			var groupAFiles = Directory.GetFiles(pathA);
			var groupBFiles = Directory.GetFiles(pathB);

			if (groupAFiles.Length == 0 || groupBFiles.Length == 0)
			{
				_logger.LogWarning("One or both image groups are empty.");
				return Enumerable.Empty<ComparisonResult>();
			}

			var results = new ConcurrentBag<ComparisonResult>();

			Parallel.ForEach(groupAFiles, fileA =>
			{
				using var imageA = LoadAndPreprocessImage(fileA, out int width, out int height);

				foreach (var fileB in groupBFiles)
				{
					using var imageB = LoadAndPreprocessImage(fileB, width, height);
					float ssim = ComputeSSIM(imageA, imageB, width, height);

					if (ssim >= _threshold)
					{
						results.Add(new ComparisonResult
						{
							A = Path.GetFileName(fileA),
							B = Path.GetFileName(fileB),
							Similarity = ssim
						});
					}
				}
			});

			_logger.LogInformation("{Count} matches found", results.Count);
			return results;
		}

		private static Image<Rgba32> LoadAndPreprocessImage(string filePath, out int width, out int height)
		{
			var image = Image.Load<Rgba32>(filePath);
			width = image.Width;
			height = image.Height;
			image.Mutate(x => x.Grayscale());
			return image;
		}

		private static Image<Rgba32> LoadAndPreprocessImage(string filePath, int width, int height)
		{
			var image = Image.Load<Rgba32>(filePath);
			image.Mutate(x => x.Resize(width, height).Grayscale());
			return image;
		}

		private static float ComputeSSIM(Image<Rgba32> imgA, Image<Rgba32> imgB, int width, int height)
		{
			float meanA = 0, meanB = 0, varA = 0, varB = 0, cov = 0;
			int count = width * height;

			// Calculate means
			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					float a = imgA[x, y].R / 255f;
					float b = imgB[x, y].R / 255f;
					meanA += a;
					meanB += b;
				}
			meanA /= count;
			meanB /= count;

			// Calculate variances and covariance
			for (int y = 0; y < height; y++)
				for (int x = 0; x < width; x++)
				{
					float a = imgA[x, y].R / 255f - meanA;
					float b = imgB[x, y].R / 255f - meanB;
					varA += a * a;
					varB += b * b;
					cov += a * b;
				}
			varA /= count;
			varB /= count;
			cov /= count;

			const float c1 = 0.01f * 0.01f;
			const float c2 = 0.03f * 0.03f;
			return (2 * meanA * meanB + c1) * (2 * cov + c2) /
				   ((meanA * meanA + meanB * meanB + c1) * (varA + varB + c2));
		}
	}
}