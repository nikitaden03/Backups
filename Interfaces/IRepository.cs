namespace Backups.Interfaces;

public interface IRepository
{
    string BaseRepositoryPath { get; }

    string PathSeparator { get; }

    IPartFileSystem OpenPartOfFileSystem(string path);

    Stream OpenWriteStream(string path);

    string GetRelativePath(string path);

    string GetFullPath(string path);

    void CreateDirectory(string path);

    void CreateFile(string path);
}