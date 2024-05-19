using System.Text.Json;

namespace ProjectB.IO;

/// <summary>
/// This class serves as the preferred method to do any object reading from JSON-files. It is one possible implementation
/// of the IFileReader.
/// </summary>
/// 
/// <typeparam name="T">The type of the objects that will be read from the file.</typeparam>
public class JsonFileReader<T> : IFileReader<T>
{
    /// <summary>
    /// Method responsible for the reading of JSON-files, and mapping the resulting objects to instances of T.
    /// </summary>
    /// 
    /// <param name="fileName">Name of the file. Should be a relative or absolute path.</param>
    /// <typeparam name="T">Type of the objects.</typeparam>
    /// 
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="fileName" /> is an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="fileName" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="fileName" /> includes an incorrect or invalid syntax for file name, directory name, or volume label.</exception>
    /// <returns>A collection containing the deserialized objects.</returns>
    public ICollection<T>? ReadAllObjects(string fileName)
    {
        if (!File.Exists(fileName)) {
            File.Create(fileName).Close();
        }
        using StreamReader reader = new StreamReader(fileName);
        string json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<List<T>>(json);
    }
}