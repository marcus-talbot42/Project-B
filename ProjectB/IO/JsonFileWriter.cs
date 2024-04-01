using Newtonsoft.Json;

namespace ProjectB.IO;

/// <summary>
/// Implements the <code>IFileWriter</code>, allowing for writing objects to a JSON-file.
/// </summary>
public class JsonFileWriter<T> : IFileWriter<T>
{
    /// <summary>
    /// Method that allows us to write a collection of objects to the JSON-file with the given name.
    /// </summary>
    /// 
    /// <exception cref="UnauthorizedAccessException">Access is denied.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
    /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
    /// <exception cref="T:System.IO.IOException">
    /// <paramref name="fileName" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    public void WriteObjects(string fileName, ICollection<T> objects)
    {
        // If the file exists, we simply open a StreamWriter. If not, we create the file, and convert the resulting FileStream to a StreamWriter.
        using StreamWriter writer = File.Exists(fileName) ? new StreamWriter(fileName) : new StreamWriter(File.Create(fileName));
        string json = JsonConvert.SerializeObject(objects);
        writer.Write(json);
    }
}