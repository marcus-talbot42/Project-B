using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Services;
using System.Collections.Generic;

namespace ProjectB.Tests
{
    [TestClass]
    public class UnitTestTranslations
    {
        [TestMethod]
        public void GetTranslationString_ReturnsCorrectTranslation_WhenKeyExists()
        {
            // Arrange
            var translation = new Translation(new Dictionary<string, string> { { "test", "Test Translation" } });
            var service = new TranslationService(new InMemoryRepository<Translation, string>(translation));

            // Act
            var result = service.GetTranslationString("test");

            // Assert
            Assert.AreEqual("Test Translation", result);
        }

        [TestMethod]
        public void GetTranslationString_ReturnsFallbackString_WhenKeyDoesNotExist()
        {
            // Arrange
            var translation = new Translation(new Dictionary<string, string> { { "test", "Test Translation" } });
            var service = new TranslationService(new InMemoryRepository<Translation, string>(translation));

            // Act
            var result = service.GetTranslationString("nonexistent");

            // Assert
            Assert.AreEqual("Translation not found: nonexistent", result);
        }
    }
}