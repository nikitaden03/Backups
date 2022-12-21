using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Models;

public class DirectoryZipObject : IZipObject
{
    public DirectoryZipObject(string name, IEnumerable<IZipObject> zipObjects)
    {
        Name = name;
        ZipObjects = zipObjects;
    }

    public string Name { get; }

    public IEnumerable<IZipObject> ZipObjects { get; }

    public IPartFileSystem GetPartFileSystem(ZipArchiveEntry zipArchiveEntry)
    {
        IReadOnlyList<IPartFileSystem> Func(ZipArchiveEntry zipArchiveStreamEntry)
        {
            var archive = new ZipArchive(zipArchiveStreamEntry.Open(), ZipArchiveMode.Read);

            return archive.Entries.Select(FindZipObject).ToList();
        }

        return new DirectoryPart(Name[.. ^4],  () => Func(zipArchiveEntry));
    }

    private IPartFileSystem FindZipObject(ZipArchiveEntry archiveEntry)
    {
        return ZipObjects.First(x => x.Name == archiveEntry.Name).GetPartFileSystem(archiveEntry);
    }
}