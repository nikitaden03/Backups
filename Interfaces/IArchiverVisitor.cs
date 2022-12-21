namespace Backups.Interfaces;

public interface IArchiverVisitor
{
    void Visit(IDirectoryPartFileSystem part);

    void Visit(IFilePartFileSystem part);
}