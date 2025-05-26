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
			// Ensure correct configuration key reading, assuming it's nested under ComparisonSettings
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

			var results = new ConcurrentBag<ComparisonResult>(); // Use ConcurrentBag for thread-safe adds

			Parallel.ForEach(groupAFiles, fileA =>
			{
				try // Add try-catch for potential image loading errors
				{
					using var imageA = Image.Load<Rgba32>(fileA);
					foreach (var fileB in groupBFiles)
					{
						try // Add try-catch for potential image loading errors
						{
							using var imageB = Image.Load<Rgba32>(fileB);
							// Clone images before passing to ComputeSSIM to ensure thread safety
							float ssim = ComputeSSIM(imageA.Clone(), imageB.Clone()); 
							if (ssim >= Threshold)
							{
								// No lock needed for ConcurrentBag
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
			return results.ToList(); // Convert back to List as required by the interface
		}

		private static float ComputeSSIM(Image<Rgba32> imgA, Image<Rgba32> imgB)
		{
			const int fixedSize = 256; // Define fixed size for resizing
			
			// Resize and convert to grayscale
			imgA.Mutate(x => x.Resize(fixedSize, fixedSize).Grayscale());
			imgB.Mutate(x => x.Resize(fixedSize, fixedSize).Grayscale());

			float meanA = 0, meanB = 0, varA = 0, varB = 0, cov = 0;
			int count = fixedSize * fixedSize; // Use fixed size for count

			// Calculate means
			for (int y = 0; y < fixedSize; y++)
			{
				for (int x = 0; x < fixedSize; x++)
				{
					// Access the R channel after grayscale conversion
					float a = imgA[x, y].R / 255f; 
					float b = imgB[x, y].R / 255f;
					meanA += a;
					meanB += b;
				}
			}

			meanA /= count;
			meanB /= count;

			// Calculate variances and covariance
			for (int y = 0; y < fixedSize; y++)
			{
				for (int x = 0; x < fixedSize; x++)
				{
					// Access the R channel after grayscale conversion
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

			// SSIM constants
			const float c1 = 0.01f * 0.01f; // (k1*L)^2, L=1 for normalized values [0,1]
			const float c2 = 0.03f * 0.03f; // (k2*L)^2, L=1 for normalized values [0,1]

			// Calculate SSIM
			// Formula: SSIM(x,y) = (2*meanX*meanY + C1) * (2*covXY + C2) / ((meanX^2 + meanY^2 + C1)*(varX + varY + C2))
			float ssimValue = (2 * meanA * meanB + c1) * (2 * cov + c2) /
						   ((meanA * meanA + meanB * meanB + c1) * (varA + varB + c2));
			
			// Ensure SSIM is within [0, 1] range, although theoretically it should be [-1, 1]
			// Clamping to [0, 1] might be safer depending on how the threshold is used.
			// return Math.Max(0f, ssimValue); // Optional: Clamp if negative values are problematic
			return ssimValue;
		}
	}
}

