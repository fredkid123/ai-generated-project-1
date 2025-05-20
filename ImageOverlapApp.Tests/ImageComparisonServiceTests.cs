using ImageOverlapApp.Services;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace ImageOverlapApp.Tests
{
	public class ImageComparisonServiceTests
	{
		private readonly string testDataPath = Path.Combine(AppContext.BaseDirectory, "TestData");

		[Fact]
		public void CompareImages_ShouldReturnMatch_WhenImagesAreSimilar()
		{
			var groupA = Path.Combine(AppContext.BaseDirectory, "tempA");
			var groupB = Path.Combine(AppContext.BaseDirectory, "tempB");

			PrepareTestImages(groupA, groupB, "imgA_0000.jpg", "imgB_0000.jpg");

			var service = new ImageComparisonService(new DummyLogger());
			var result = service.CompareImages(groupA, groupB).ToList();

			Assert.Single(result);
			Assert.Equal("imgA_0000.jpg", result[0].A);
			Assert.Equal("imgB_0000.jpg", result[0].B);
		}

		[Fact]
		public void CompareImages_ShouldReturnEmpty_WhenImagesAreDifferent()
		{
			var groupA = Path.Combine(AppContext.BaseDirectory, "tempA2");
			var groupB = Path.Combine(AppContext.BaseDirectory, "tempB2");

			PrepareTestImages(groupA, groupB, "imgA_0000.jpg", "imgB_0001.jpg");

			var service = new ImageComparisonService(new DummyLogger());
			var result = service.CompareImages(groupA, groupB).ToList();

			Assert.Empty(result);
		}

		private void PrepareTestImages(string groupA, string groupB, string fileA, string fileB)
		{
			if (Directory.Exists(groupA))
			{
				Directory.Delete(groupA, true);
			}
			if (Directory.Exists(groupB))
			{
				Directory.Delete(groupB, true);
			}
			Directory.CreateDirectory(groupA);
			Directory.CreateDirectory(groupB);

			File.Copy(Path.Combine(testDataPath, fileA), Path.Combine(groupA, fileA));
			File.Copy(Path.Combine(testDataPath, fileB), Path.Combine(groupB, fileB));
		}

		private class DummyLogger : Microsoft.Extensions.Logging.ILogger<ImageOverlapApp.Services.ImageComparisonService>
		{
			public IDisposable BeginScope<TState>(TState state) => null;
			public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => false;
			public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
		}
	}
}