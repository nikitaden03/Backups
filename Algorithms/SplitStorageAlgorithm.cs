using Backups.Interfaces;
using Backups.Models;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IStorageAlgorithm
{
    private readonly IArchiver _archiver;

    public SplitStorageAlgorithm(IArchiver archiver)
    {
        _archiver = archiver;
    }

    public IStorage CreateStorage(IReadOnlyList<IPartFileSystem> partFileSystems, string pathToStorageDir, IRepository repository)
    {
        var storages = partFileSystems.Select(partFileSystem => _archiver.Zip(new List<IPartFileSystem> { partFileSystem }, repository, pathToStorageDir)).ToList();

        return new SplitStorage(storages);
    }

    public override string ToString()
    {
        return _archiver.ToString() ?? string.Empty;
    }
}