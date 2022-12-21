using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;
using Zio;
using Zio.FileSystems;

namespace Backups.Entities;

public class InMemoryRepository : IRepository, IDisposable
{
    public InMemoryRepository(string root)
    {
        BaseRepositoryPath = $@"\mnt\c\{root}";
        FileSystem = new MemoryFileSystem();
        FileSystem.CreateDirectory(BaseRepositoryPath);
        PathSeparator = @"\";
    }

    public string BaseRepositoryPath { get; }
    public string PathSeparator { get; }

    public MemoryFileSystem FileSystem { get; }

    public IPartFileSystem OpenPartOfFileSystem(string path)
    {
        string fullPath = GetFullPath(path);
        if (FileSystem.FileExists(fullPath))
        {
            return OpenFile(fullPath, path);
        }

        if (FileSystem.DirectoryExists(fullPath))
        {
            return OpenDirectory(fullPath, path);
        }

        throw RepositoryException.FileDoesNotExist();
    }

    public Stream OpenWriteStream(string path)
    {
        string fullPath = GetFullPath(path);
        if (FileSystem.FileExists(fullPath))
        {
            return FileSystem.OpenFile((UPath)fullPath, FileMode.Open, FileAccess.Write);
        }

        throw RepositoryException.FileDoesNotExist();
    }

    public string GetRelativePath(string path)
    {
        string relativePath = path[(BaseRepositoryPath.Length + 1) ..];
        if (!(FileSystem.FileExists(GetFullPath(relativePath)) || FileSystem.DirectoryExists(GetFullPath(relativePath))))
        {
            throw RepositoryException.FileIsNotInRepository();
        }

        return relativePath;
    }

    public string GetFullPath(string path)
    {
        return Path.Combine(BaseRepositoryPath, path);
    }

    public void CreateDirectory(string path)
    {
        string fullPath = GetFullPath(path);
        if (!FileSystem.DirectoryExists(fullPath))
        {
            FileSystem.CreateDirectory(fullPath);
        }
        else
        {
            throw RepositoryException.DirectoryAlreadyExist();
        }
    }

    public void CreateFile(string path)
    {
        string fullPath = GetFullPath(path);
        if (!FileSystem.FileExists(fullPath))
        {
            FileSystem.CreateFile(fullPath).Close();
        }
        else
        {
            throw RepositoryException.FileAlreadyExist();
        }
    }

    public void Dispose()
    {
        FileSystem.Dispose();
    }

    private IPartFileSystem OpenFile(string fullPath, string relativePath)
    {
        return new FilePart(relativePath.Split(@"\")[^1], () => FileSystem.OpenFile((UPath)fullPath, FileMode.Open, FileAccess.Read));
    }

    private IPartFileSystem OpenDirectory(string fullPath, string relativePath)
    {
        var partOfFileSystems = FileSystem.GetDirectoryEntry((UPath)fullPath).EnumerateFiles().Select(fileEntry => OpenFile(fileEntry.FullName, $@"{relativePath}\{fileEntry.Name.Split(@"\")[^1]}")).ToList();
        partOfFileSystems.AddRange(FileSystem.GetDirectoryEntry((UPath)fullPath).EnumerateDirectories().Select(directoryEntry => OpenDirectory(directoryEntry.FullName, $@"{relativePath}\{directoryEntry.Name.Split(@"\")[^1]}")));
        return new DirectoryPart(relativePath.Split(@"\")[^1], () => partOfFileSystems);
    }
}