namespace ProjectB.IO;

/// <summary>
/// This interface serves as the blueprint for all file writers. Currently, we only have need for JSON writing, however,
/// that may change in the future. Programming using this interface, rather than the implementations, allows us to switch
/// between necessary implementations easily, without changing the implementation of the calling class.
/// </summary>
/// 
/// <typeparam name="T">The type of the objects that will be written to the file.</typeparam>
public interface IFileWriter<T>
{
    /// <summary>
    /// This method allows us to write objects to a given file. The filetype and writing strategy depends on the class
    /// implementing this method.
    /// </summary>
    /// 
    /// <param name="fileName">The name of the file. Should be either a relative path, or an absolute path.</param>
    /// <param name="objects">The collection of objects that will be written to the specified file. All objects should
    /// be of type T</param>
    /// <typeparam name="T">The type of the objects that will be written to the file.</typeparam>
    void WriteObjects(string fileName, ICollection<T> objects);
}