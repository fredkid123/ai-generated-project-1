using System.IO;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using ImageOverlapApp.Services;

namespace ImageOverlapApp.Tests
{
	public class ImageComparisonServiceTests
	{
		[Fact]
		public void CompareImages_ShouldReturnMatchingPairs_WhenOverlapDetected()
		{
			// Arrange
			var loggerMock = new Mock<ILogger<ImageComparisonService>>();
			var service = new ImageComparisonService(loggerMock.Object);

			// Copie duas imagens similares para essas pastas para testar
			var groupADir = "TestData/groupA";
			var groupBDir = "TestData/groupB";

			Directory.CreateDirectory(groupADir);
			Directory.CreateDirectory(groupBDir);

			// Use a mesma imagem para garantir similaridade
			File.Copy("TestData/imgA_0000.jpg", Path.Combine(groupADir, "a1.jpg"), overwrite: true);
			File.Copy("TestData/imgB_0000.jpg", Path.Combine(groupBDir, "b1.jpg"), overwrite: true);

			// Act
			var matches = service.CompareImages(groupADir, groupBDir);

			// Assert
			Assert.Contains(matches, m => m.A == "a1.jpg" && m.B == "b1.jpg");
		}
	}
}