using Backups.Interfaces;

namespace Backups.Models;

public class FilePart : IFilePartFileSystem
{
    private readonly Func<Stream> _stream;

    public FilePart(string name, Func<Stream> stream)
    {
        Name = name;
        _stream = stream;
    }

    public string Name { get; }

    public Stream Stream => _stream();

    public void Accept(IArchiverVisitor archiverVisitor)
    {
        archiverVisitor.Visit(this);
    }
}