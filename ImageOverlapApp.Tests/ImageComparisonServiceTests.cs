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
			var pathServiceMock = new Mock<IPathService>();
			var loggerMock = new Mock<ILogger<ImageComparisonService>>();
			var configServiceMock = new Mock<IConfiguration>();
			var sectionMock = new Mock<IConfigurationSection>();
			sectionMock.Setup(c => c.Value).Returns((string)null);
			configServiceMock.Setup(c => c.GetSection(It.IsAny<string>())).Returns(sectionMock.Object);
			configServiceMock.Setup(c => c["ComparisonSettings:SsimThreshold"]).Returns("0.85");
			return new ImageComparisonService(loggerMock.Object, configServiceMock.Object, pathServiceMock.Object);
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

			var result = service.CompareImages(groupA, groupB, "");

			Assert.Contains(result, r => r.A.Contains("imgA_0000") && r.B.Contains("imgB_0000"));
		}

		[Fact]
		public void CompareImages_ShouldNotReturnPair_WhenNoOverlap()
		{
			var service = CreateService();
			var groupA = "TestData/groupA";
			var groupB = "TestData/groupB";

			PrepareTestImages(groupA, groupB, "imgA_0000.jpg", "imgB_0001.jpg");

			var result = service.CompareImages(groupA, groupB, "");

			Assert.DoesNotContain(result, r => r.A.Contains("imgA_0000") && r.B.Contains("imgB_0001"));
		}
	}
}
