namespace ProjectB.IO;

/// <summary>
/// Interface serving as the blueprint for all kinds of file readers. We currently only have need for JSON-reading, this
/// ,may change in the future. This interface will allow us to switch out readers easily, by programming to the interface,
/// rather than to the implementation.
/// </summary>
/// <typeparam name="T">The type that the retrieved objects will be mapped to.</typeparam>
public interface IFileReader<T>
{
    /// <summary>
    ///
    /// This method allows us to read all objects in a file. The actual file, and reading strategy depends on the
    /// class implementing this method.
    /// </summary>
    ///<param name="fileName">The name of the file on the filesystem. Should be either a relative path, or an absolute
    /// path.</param>.
    /// <typeparam name="T">The type of the objects that will be read from the file.</typeparam>
    /// <returns>Type-safe collection of objects.</returns>
    ICollection<T>? ReadAllObjects(string fileName);
}