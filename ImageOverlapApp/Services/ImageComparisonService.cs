using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static System.Net.Mime.MediaTypeNames;

namespace ImageOverlapApp.Services
{
	public class ImageComparisonService : IImageComparisonService
	{
		private ILogger<ImageComparisonService> Logger { get; set; }

		public ImageComparisonService(ILogger<ImageComparisonService> logger)
		{
			Logger = logger;
		}

		public IEnumerable<(string A, string B)> CompareImages(string groupADir, string groupBDir)
		{
			var groupAFiles = Directory.GetFiles(groupADir);
			var groupBFiles = Directory.GetFiles(groupBDir);

			var matches = new List<(string A, string B)>();

			foreach (var fileA in groupAFiles)
			{
				foreach (var fileB in groupBFiles)
				{
					if (HasVisualOverlap(fileA, fileB))
					{
						matches.Add((Path.GetFileName(fileA), Path.GetFileName(fileB)));
					}
				}
			}

			Logger.LogInformation("Encontrados {count} pares semelhantes.", matches.Count);
			return matches;
		}

		private bool HasVisualOverlap(string pathA, string pathB)
		{
			try
			{
				using var imgA = Image.Load<Rgba32>(pathA);
				using var imgB = Image.Load<Rgba32>(pathB);

				imgA.Mutate(x => x.Resize(100, 100).Grayscale());
				imgB.Mutate(x => x.Resize(100, 100).Grayscale());

				double diff = ComputeDifference(imgA, imgB);
				Logger.LogDebug("Diferença entre {A} e {B} = {Diff}", Path.GetFileName(pathA), Path.GetFileName(pathB), diff);
				return diff < 0.2;
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Erro ao comparar {A} e {B}", pathA, pathB);
				return false;
			}
		}

		private static double ComputeDifference(Image<Rgba32> a, Image<Rgba32> b)
		{
			double total = 0;
			for (int y = 0; y < a.Height; y++)
			{
				Span<Rgba32> rowA = a.GetPixelRowSpan(y);
				Span<Rgba32> rowB = b.GetPixelRowSpan(y);
				for (int x = 0; x < a.Width; x++)
				{
					var grayA = rowA[x].R / 255.0;
					var grayB = rowB[x].R / 255.0;
					total += Math.Pow(grayA - grayB, 2);
				}
			}
			return total / (a.Width * a.Height);
		}
	}
}
