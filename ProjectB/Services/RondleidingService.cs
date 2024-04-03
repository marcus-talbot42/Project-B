using System;
using ProjectB.IO;

namespace ProjectB.Services
{
    // Naam van de serve moet nog aangepast worden
    public class RondleidingService
    {
        private readonly JsonFileReader _jsonFileReader;

        public RondleidingService(JsonFileReader jsonFileReader)
        {
            _jsonFileReader = jsonFileReader;
        }

        public void ShowRondleidingData()
        {
            // Replace "path/to/your/json/file.json" with the actual path to your JSON file
            string jsonFilePath = "path/to/your/json/file.json";

            try
            {
                // Read the JSON file using the JsonFileReader
                var jsonData = _jsonFileReader.ReadJsonFile(jsonFilePath);

                // Display the data in the console
                Console.WriteLine(jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}