using System.IO.Compression;
using Backups.Entities;
using Backups.Interfaces;

namespace Backups.Models;

public class Archiver : IArchiver
{
    private string _logMessage = string.Empty;

    public IStorage Zip(List<IPartFileSystem> partFileSystems, IRepository repository, string pathToStorageDir)
    {
        var storageGuid = Guid.NewGuid();
        string relativePath = $@"{pathToStorageDir}{repository.PathSeparator}{storageGuid}.zip";
        repository.CreateFile(relativePath);
        using Stream stream = repository.OpenWriteStream(relativePath);

        var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        var visitor = new ZipArchiverVisitor(archive);

        foreach (IPartFileSystem partFileSystem in partFileSystems)
        {
            partFileSystem.Accept(visitor);
        }

        var zipObjects = visitor.GetZipObjects.ToList();
        archive.Dispose();

        _logMessage = storageGuid.ToString();

        return new Storage(new DirectoryZipObject(storageGuid.ToString(), zipObjects), repository, @$"{pathToStorageDir}\{storageGuid}.zip");
    }

    public override string ToString()
    {
        return _logMessage;
    }
}