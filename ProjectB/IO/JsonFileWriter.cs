using Newtonsoft.Json;

namespace ProjectB.IO;

public class JsonFileWriter<T> : IFileWriter<T>
{
    public void WriteObjects(string fileName, ICollection<T> objects)
    {
        using StreamWriter writer = new StreamWriter(fileName);
        string json = JsonConvert.SerializeObject(objects);
        writer.Write(json);
    }
}