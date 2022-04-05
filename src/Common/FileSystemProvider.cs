namespace Common;

public class FileSystemProvider : IFileSystemProvider
{
    public bool Exists(string filename)
    {
        return File.Exists(filename);
    }

    public Stream Read(string filename)
    {
        return File.Open(filename, FileMode.Open, FileAccess.Read);
    }

    public Task WriteAsync(string filename, Stream stream)
    {
        _ = stream.Seek(0, SeekOrigin.Begin);

        using var sr = new StreamReader(stream);
        var text = sr.ReadToEnd();

        using var outputStream = File.Open(filename, FileMode.Truncate, FileAccess.Write);
        using var sw = new StreamWriter(outputStream);

        return sw.WriteAsync(text);

    }
}
