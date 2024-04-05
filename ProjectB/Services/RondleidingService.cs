using System;
using ProjectB.IO;
using ProjectB.Models;
using System.IO;
using Newtonsoft.Json;

namespace ProjectB.Services
{
    public class RondleidingService
    {
        private readonly JsonFileReader<Tour> _jsonFileReader; // Specify Tour as the type argument

        public RondleidingService(JsonFileReader<Tour> jsonFileReader) // Adjust the constructor parameter
        {
            _jsonFileReader = jsonFileReader;
        }

        public void ShowRondleidingData()
        {
            // Replace "path/to/your/json/file.json" with the actual path to your JSON file
            string jsonFilePath = "C:\\Bureaublad\\C# Main\\Project-B\\ProjectB\\Database\\database.json";

            try
            {
                // Read the JSON file using the JsonFileReader<Tour>
                var tourData = _jsonFileReader.ReadJsonFile(jsonFilePath);

                // Display the data in the console
                Console.WriteLine(tourData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    public class JsonFileReader<T>
    {
        public T ReadJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("JSON file not found.", filePath);
            }

            string jsonData = File.ReadAllText(filePath);
            T deserializedData = JsonConvert.DeserializeObject<T>(jsonData);
            return deserializedData;
        }
    }
}
