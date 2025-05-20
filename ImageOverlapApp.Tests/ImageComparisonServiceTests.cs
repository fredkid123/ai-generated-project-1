using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ImageOverlapApp.Services;

namespace ImageOverlapApp.Tests
{
	public class ImageComparisonServiceTests
	{
		[Fact]
		public void CompareImages_ShouldReturnMatchingPairs_WhenOverlapDetected()
		{
			var loggerMock = new Mock<ILogger<ImageComparisonService>>();
			var configMock = new Mock<IConfiguration>();
			configMock.Setup(c => c["SimilarityThreshold"]).Returns("0.85");

			var service = new ImageComparisonService(loggerMock.Object, configMock.Object);

			var groupA = "TestData/groupA";
			var groupB = "TestData/groupB";

			Directory.CreateDirectory(groupA);
			Directory.CreateDirectory(groupB);
			File.Copy("TestData/imgA_0000.jpg", Path.Combine(groupA, "imgA_0000.jpg"), true);
			File.Copy("TestData/imgB_0000.jpg", Path.Combine(groupB, "imgB_0000.jpg"), true);

			var result = service.CompareImages(groupA, groupB);

			Assert.Contains(result, r => r.A.Contains("imgA_0000") && r.B.Contains("imgB_0000"));
		}
	}
}
