using Backups.Interfaces;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IStorageAlgorithm
{
    private readonly IArchiver _archiver;

    public SingleStorageAlgorithm(IArchiver archiver)
    {
        _archiver = archiver;
    }

    public IStorage CreateStorage(IReadOnlyList<IPartFileSystem> partFileSystems, string pathToStorageDir, IRepository repository)
    {
        IStorage storage = _archiver.Zip(partFileSystems.ToList(), repository, pathToStorageDir);
        return storage;
    }

    public override string ToString()
    {
        return _archiver.ToString() ?? string.Empty;
    }
}