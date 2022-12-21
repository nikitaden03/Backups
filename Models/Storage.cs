using System.IO.Compression;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Models;

public class Storage : IStorage
{
    private DirectoryZipObject _zipObjects;

    public Storage(DirectoryZipObject zipObjects, IRepository repository, string repositoryPath)
    {
        RepositoryPath = repositoryPath;
        _zipObjects = zipObjects;
        Repository = repository;
    }

    public string RepositoryPath { get; }

    public IRepository Repository { get; }

    public IDisposablePartFileSystem GetDisposablePartFileSystem()
    {
        IPartFileSystem partFileSystem = Repository.OpenPartOfFileSystem(RepositoryPath);
        if (partFileSystem is not IFilePartFileSystem filePartFileSystem)
        {
            throw TypeException.PartFileSystemIsNotFile();
        }

        var archive = new ZipArchive(filePartFileSystem.Stream, ZipArchiveMode.Read);
        return new DisposablePartFileSystem(_zipObjects.ZipObjects, archive);
    }
}