using ImageOverlapApp.Models;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO; // Added for Path.GetFileName
using System.Collections.Generic; // Added for List
using System.Linq; // Added for Enumerable
using System.Threading.Tasks; // Added for Parallel.ForEach
using Microsoft.Extensions.Configuration; // Added for IConfiguration
using System;
using System.Collections.Concurrent; // Added for ConcurrentBag

namespace ImageOverlapApp.Services
{
	public class ImageComparisonService : IImageComparisonService
	{
		private ILogger<ImageComparisonService> Logger { get; set; }
		private float Threshold { get; set; }
		private IPathService PathService { get; set; }

		public ImageComparisonService(ILogger<ImageComparisonService> logger, IConfiguration config, IPathService pathService)
		{
			Logger = logger;
			Threshold = config.GetValue<float>("ComparisonSettings:SimilarityThreshold", 0.85f); 
			PathService = pathService;
		}

		public IEnumerable<ComparisonResult> CompareImages(string groupA, string groupB, string instanceId)
		{
			Logger.LogInformation("Comparando imagens usando SSIM com threshold {Threshold}", Threshold);

			string pathA = PathService.GetGroupPath(groupA, instanceId);
			string pathB = PathService.GetGroupPath(groupB, instanceId);

			if (!Directory.Exists(pathA) || !Directory.Exists(pathB))
			{
				Logger.LogWarning("Um dos grupos de imagem nao foi encontrado: pathA={PathA}, pathB={PathB}", pathA, pathB);
				return Enumerable.Empty<ComparisonResult>();
			}

			var groupAFiles = Directory.GetFiles(pathA);
			var groupBFiles = Directory.GetFiles(pathB);

			var results = new ConcurrentBag<ComparisonResult>();

			Parallel.ForEach(groupAFiles, fileA =>
			{
				try
				{
					using var imageA = Image.Load<Rgba32>(fileA);
					foreach (var fileB in groupBFiles)
					{
						try
						{
							using var imageB = Image.Load<Rgba32>(fileB);
							float ssim = ComputeSSIM(imageA.Clone(), imageB.Clone()); 
							if (ssim >= Threshold)
							{
								results.Add(new ComparisonResult
								{
									A = Path.GetFileName(fileA),
									B = Path.GetFileName(fileB),
									Similarity = ssim
								});
							}
						}
						catch (Exception ex)
						{
							Logger.LogError(ex, "Erro ao processar imagem B: {FilePathB}", fileB);
						}
					}
				}
				catch (Exception ex)
				{
					Logger.LogError(ex, "Erro ao processar imagem A: {FilePathA}", fileA);
				}
			});

			Logger.LogInformation("{Count} correspondencias encontradas", results.Count);
			return results.ToList();
		}

		private static float ComputeSSIM(Image<Rgba32> imgA, Image<Rgba32> imgB)
		{
			// Resize to Max dimensions as suggested by user
			int width = Math.Max(imgA.Width, imgB.Width);
			int height = Math.Max(imgA.Height, imgB.Height);
			
			// Resize and convert to grayscale
			imgA.Mutate(x => x.Resize(width, height).Grayscale());
			imgB.Mutate(x => x.Resize(width, height).Grayscale());

			float meanA = 0, meanB = 0, varA = 0, varB = 0, cov = 0;
			int count = width * height; // Use max dimensions for count

			// Calculate means
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float a = imgA[x, y].R / 255f; 
					float b = imgB[x, y].R / 255f;
					meanA += a;
					meanB += b;
				}
			}

			meanA /= count;
			meanB /= count;

			// Calculate variances and covariance
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float a = imgA[x, y].R / 255f - meanA; 
					float b = imgB[x, y].R / 255f - meanB;
					varA += a * a;
					varB += b * b;
					cov += a * b;
				}
			}

			varA /= count;
			varB /= count;
			cov /= count;

			const float c1 = 0.01f * 0.01f;
			const float c2 = 0.03f * 0.03f;

			float ssimValue = (2 * meanA * meanB + c1) * (2 * cov + c2) /
						   ((meanA * meanA + meanB * meanB + c1) * (varA + varB + c2));
			
			return ssimValue;
		}
	}
}

