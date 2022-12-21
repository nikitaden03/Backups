namespace Backups.Interfaces;

public interface IStorage
{
    IDisposablePartFileSystem GetDisposablePartFileSystem();
}