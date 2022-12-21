using System.IO.Compression;
using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities;

public class ZipArchiverVisitor : IArchiverVisitor, IDisposable
{
    private readonly Stack<List<IZipObject>> _zipObjects;

    private readonly Stack<ZipArchive> _zipArchives;

    public ZipArchiverVisitor(ZipArchive archive)
    {
        _zipArchives = new Stack<ZipArchive>();
        _zipObjects = new Stack<List<IZipObject>>();
        _zipObjects.Push(new List<IZipObject>());
        _zipArchives.Push(archive);
    }

    public IEnumerable<IZipObject> GetZipObjects => _zipObjects.Peek();

    public void Visit(IDirectoryPartFileSystem part)
    {
        ZipArchive zipArchive = _zipArchives.Peek();
        ZipArchiveEntry zipArchiveEntry = zipArchive.CreateEntry($@"{part.Name}.zip");
        using Stream stream = zipArchiveEntry.Open();
        var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        _zipArchives.Push(archive);
        _zipObjects.Push(new List<IZipObject>());
        foreach (IPartFileSystem partFileSystem in part.Children)
        {
            partFileSystem.Accept(this);
        }

        List<IZipObject> children = _zipObjects.Pop();
        _zipObjects.Peek().Add(new DirectoryZipObject($"{part.Name}.zip", children));

        _zipArchives.Pop().Dispose();
    }

    public void Visit(IFilePartFileSystem part)
    {
        ZipArchive archive = _zipArchives.Peek();
        ZipArchiveEntry entry = archive.CreateEntry(part.Name);
        using Stream stream = entry.Open();
        using Stream fileStream = part.Stream;
        fileStream.CopyTo(stream);
        _zipObjects.Peek().Add(new FileZipObjects(part.Name));
    }

    public void Visit(IPartFileSystem part)
    {
        part.Accept(this);
    }

    public void Dispose()
    {
        _zipArchives.Pop().Dispose();
    }
}