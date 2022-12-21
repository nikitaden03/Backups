using Backups.Interfaces;

namespace Backups.Models;

public class DirectoryPart : IDirectoryPartFileSystem
{
    private readonly Func<IReadOnlyList<IPartFileSystem>> _children;

    public DirectoryPart(string name, Func<IReadOnlyList<IPartFileSystem>> children)
    {
        Name = name;
        _children = children;
    }

    public string Name { get; }

    public IReadOnlyList<IPartFileSystem> Children => _children();

    public void Accept(IArchiverVisitor archiverVisitor)
    {
        archiverVisitor.Visit(this);
    }
}