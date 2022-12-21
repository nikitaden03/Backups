namespace Backups.Interfaces;

public interface IStorageAlgorithm
{
    IStorage CreateStorage(IReadOnlyList<IPartFileSystem> partFileSystems, string pathToStorageDir, IRepository repository);
}