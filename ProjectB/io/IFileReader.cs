namespace ProjectB.io;

public interface IFileReader<T>
{
    ICollection<T> ReadAllObjects(string fileName);
}