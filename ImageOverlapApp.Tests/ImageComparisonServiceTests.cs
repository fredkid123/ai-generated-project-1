using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ImageOverlapApp.Services;

namespace ImageOverlapApp.Tests
{
	public class ImageComparisonServiceTests
	{
		private ImageComparisonService CreateService()
		{
			var loggerMock = new Mock<ILogger<ImageComparisonService>>();
			var configMock = new Mock<IConfiguration>();
		configMock.Setup(c => c.GetValue<float>("ComparisonSettings:SsimThreshold", 0.85f)).Returns(0.85f);
			configMock.Setup(c => c["SimilarityThreshold"]).Returns("0.85");
			return new ImageComparisonService(loggerMock.Object, configMock.Object);
		}

		private void PrepareTestImages(string groupAPath, string groupBPath, string fileA, string fileB)
		{
			Directory.CreateDirectory(groupAPath);
			Directory.CreateDirectory(groupBPath);
			File.Copy($"TestData/{fileA}", Path.Combine(groupAPath, fileA), true);
			File.Copy($"TestData/{fileB}", Path.Combine(groupBPath, fileB), true);
		}

		[Fact]
		public void CompareImages_ShouldReturnMatchingPairs_WhenOverlapDetected()
		{
			var service = CreateService();
			var groupA = "TestData/groupA";
			var groupB = "TestData/groupB";

			PrepareTestImages(groupA, groupB, "imgA_0000.jpg", "imgB_0000.jpg");

			var result = service.CompareImages(groupA, groupB);

			Assert.Contains(result, r => r.A.Contains("imgA_0000") && r.B.Contains("imgB_0000"));
		}

		[Fact]
		public void CompareImages_ShouldNotReturnPair_WhenNoOverlap()
		{
			var service = CreateService();
			var groupA = "TestData/groupA";
			var groupB = "TestData/groupB";

			PrepareTestImages(groupA, groupB, "imgA_0000.jpg", "imgB_0001.jpg");

			var result = service.CompareImages(groupA, groupB);

			Assert.DoesNotContain(result, r => r.A.Contains("imgA_0000") && r.B.Contains("imgB_0001"));
		}
	}
}



		private string[] PrepareTestImages(string source1, string source2, string dest1, string dest2)
		{
			var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(tempDir);
			var pathA = Path.Combine(tempDir, dest1);
			var pathB = Path.Combine(tempDir, dest2);
			File.Copy(source1, pathA, true);
			File.Copy(source2, pathB, true);
			return new[] { pathA, pathB };
		}
