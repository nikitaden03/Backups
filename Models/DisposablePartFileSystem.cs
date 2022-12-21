using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Models;

public class DisposablePartFileSystem : IDisposablePartFileSystem
{
    private List<IZipObject> _zipObjects;

    private ZipArchive _archive;

    private List<IPartFileSystem> _partFileSystems;

    public DisposablePartFileSystem(IEnumerable<IZipObject> zipObjects, ZipArchive archive)
    {
        _zipObjects = zipObjects.ToList();
        _archive = archive;

        _partFileSystems = new List<IPartFileSystem>();
        foreach (ZipArchiveEntry zipArchiveEntry in _archive.Entries)
        {
            _partFileSystems.Add(_zipObjects.First(x => x.Name == zipArchiveEntry.Name).GetPartFileSystem(zipArchiveEntry));
        }
    }

    public IEnumerable<IPartFileSystem> PartFileSystems => _partFileSystems;

    public void Dispose()
    {
        _archive.Dispose();
    }
}