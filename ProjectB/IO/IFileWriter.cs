namespace ProjectB.IO;

public interface IFileWriter<T>
{

    void WriteObjects(string fileName, ICollection<T> objects);

}