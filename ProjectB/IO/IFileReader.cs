namespace ProjectB.IO;

public interface IFileReader<T>
{
    ICollection<T>? ReadAllObjects(string fileName);
}