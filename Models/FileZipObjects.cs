using System.IO.Compression;
using Backups.Interfaces;

namespace Backups.Models;

public class FileZipObjects : IZipObject
{
    public FileZipObjects(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public IPartFileSystem GetPartFileSystem(ZipArchiveEntry zipArchiveEntry)
    {
        return new FilePart(Name, zipArchiveEntry.Open);
    }
}