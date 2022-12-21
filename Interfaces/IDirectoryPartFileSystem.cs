namespace Backups.Interfaces;

public interface IDirectoryPartFileSystem : IPartFileSystem
{
    IReadOnlyList<IPartFileSystem> Children { get; }
}