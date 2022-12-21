namespace Backups.Interfaces;

public interface IPartFileSystem
{
    string Name { get; }

    void Accept(IArchiverVisitor archiverVisitor);
}