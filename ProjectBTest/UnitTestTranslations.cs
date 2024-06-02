using ProjectB.Models;
using ProjectB.Services;

namespace ProjectB.Tests
{
    [TestClass]
    public class UnitTestTranslations
    {
        [TestMethod]
        public void GetTranslationString_ReturnsCorrectTranslation_WhenKeyExists()
        {
            // Arrange
            var lang = Enums.Language.ToString();
            var translation = new Translation("NL", new Dictionary<string, string> { { "test", "This is a test." } });
            var repository = new InMemoryRepository<Translation, string>();
            repository.Save(translation);

            // Check if the translation is saved correctly
            var savedTranslation = repository.FindById("NL");
            Assert.IsNotNull(savedTranslation);
            Assert.AreEqual("This is a test.", savedTranslation.GetPairs()["test"]);

            var service = new TranslationService(repository);

            // Act
            var result = service.GetTranslationString("test");

            // Assert
            Assert.AreEqual("This is a test.", result);
        }

        [TestMethod]
        public void GetTranslationString_ReturnsFallbackString_WhenKeyDoesNotExist()
        {
            // Arrange
            var translation = new Translation("EN", new Dictionary<string, string> { { "test", "This is a test." } });
            var repository = new InMemoryRepository<Translation, string>();
            repository.Save(translation);
            var service = new TranslationService(repository);

            // Act
            var result = service.GetTranslationString("nonexistent");

            // Assert
            Assert.AreEqual("Translation not found: nonexistent", result);
        }
    }
}