namespace Backups.Interfaces;

public interface IFilePartFileSystem : IPartFileSystem
{
    Stream Stream { get; }
}