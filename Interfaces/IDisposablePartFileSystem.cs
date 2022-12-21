namespace Backups.Interfaces;

public interface IDisposablePartFileSystem : IDisposable
{
    IEnumerable<IPartFileSystem> PartFileSystems { get; }
}