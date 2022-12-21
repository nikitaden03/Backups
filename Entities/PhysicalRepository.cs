using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities;

public class PhysicalRepository : IRepository
{
    public PhysicalRepository(string baseRepositoryPath)
    {
        BaseRepositoryPath = baseRepositoryPath;
        if (!Directory.Exists(BaseRepositoryPath))
        {
            Directory.CreateDirectory(BaseRepositoryPath);
        }

        PathSeparator = Path.DirectorySeparatorChar.ToString();
    }

    public string BaseRepositoryPath { get; }
    public string PathSeparator { get; }

    public IPartFileSystem OpenPartOfFileSystem(string path)
    {
        string fullPath = GetFullPath(path);
        if (File.Exists(fullPath))
        {
            return OpenFile(fullPath, path);
        }

        if (Directory.Exists(fullPath))
        {
            return OpenDirectory(fullPath, path);
        }

        throw RepositoryException.FileDoesNotExist();
    }

    public Stream OpenWriteStream(string path)
    {
        string fullPath = GetFullPath(path);
        if (File.Exists(fullPath))
        {
            return File.Open(fullPath, FileMode.Open, FileAccess.Write);
        }

        throw RepositoryException.FileDoesNotExist();
    }

    public string GetRelativePath(string path)
    {
        string relativePath = path[(BaseRepositoryPath.Length + 1) ..];
        if (!(File.Exists(GetFullPath(relativePath)) || Directory.Exists(GetFullPath(relativePath))))
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
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        else
        {
            throw RepositoryException.DirectoryAlreadyExist();
        }
    }

    public void CreateFile(string path)
    {
        string fullPath = GetFullPath(path);
        if (!File.Exists($@"{fullPath}"))
        {
            File.Create($@"{fullPath}").Close();
        }
        else
        {
            throw RepositoryException.FileAlreadyExist();
        }
    }

    private IPartFileSystem OpenFile(string fullPath, string relativePath)
    {
        return new FilePart(relativePath.Split(@"\")[^1], () => File.Open(fullPath,  FileMode.Open, FileAccess.Read));
    }

    private IPartFileSystem OpenDirectory(string fullPath, string relativePath)
    {
        var partOfFileSystems = Directory.GetFiles(fullPath).Select(fileName => OpenFile($@"{fileName}", $@"{relativePath}\{fileName.Split(@"\")[^1]}")).ToList();
        partOfFileSystems.AddRange(Directory.GetDirectories(fullPath).Select(dirName => OpenDirectory($@"{dirName}", $@"{relativePath}\{dirName.Split(@"\")[^1]}")));
        return new DirectoryPart(relativePath.Split(@"\")[^1], () => partOfFileSystems);
    }
}