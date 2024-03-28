using Newtonsoft.Json;

namespace ProjectB.IO;

public class JsonFileReader<T> : IFileReader<T>
{
    public ICollection<T>? ReadAllObjects(string fileName)
    {
        using StreamReader reader = new StreamReader(fileName);
        string json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<List<T>>(json);
    }
}