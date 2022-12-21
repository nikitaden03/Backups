namespace Backups.Interfaces;

public interface IArchiver
{
    IStorage Zip(List<IPartFileSystem> partFileSystems, IRepository repository, string pathToStorageDir);
}