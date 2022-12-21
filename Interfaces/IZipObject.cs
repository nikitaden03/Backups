using System.IO.Compression;

namespace Backups.Interfaces;

public interface IZipObject
{
    string Name { get; }

    IPartFileSystem GetPartFileSystem(ZipArchiveEntry zipArchiveEntry);
}