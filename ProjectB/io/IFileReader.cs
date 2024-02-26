namespace ProjectB.io;

public interface IFileReader<T>
{
    T ReadObject();
    ICollection<T> ReadAllObjects();
}