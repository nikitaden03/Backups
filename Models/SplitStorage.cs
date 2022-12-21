using Backups.Interfaces;

namespace Backups.Models;

public class SplitStorage : IStorage
{
    public SplitStorage(IEnumerable<IStorage> storages)
    {
        Storages = storages;
    }

    public IEnumerable<IStorage> Storages { get; }

    public IDisposablePartFileSystem GetDisposablePartFileSystem()
    {
        var disposablePartFileSystems = new List<IDisposablePartFileSystem>();
        foreach (IStorage storage in Storages)
        {
            disposablePartFileSystems.Add(storage.GetDisposablePartFileSystem());
        }

        return new DisposablePartFileSystemAdapter(disposablePartFileSystems);
    }
}