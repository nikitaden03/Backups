using Backups.Interfaces;

namespace Backups.Models;

public class DisposablePartFileSystemAdapter : IDisposablePartFileSystem
{
    private IEnumerable<IDisposablePartFileSystem> _disposablePartFileSystems;

    public DisposablePartFileSystemAdapter(IEnumerable<IDisposablePartFileSystem> disposablePartFileSystems)
    {
        _disposablePartFileSystems = disposablePartFileSystems;
    }

    public IEnumerable<IPartFileSystem> PartFileSystems =>
        _disposablePartFileSystems.SelectMany(x => x.PartFileSystems);

    public void Dispose()
    {
        foreach (IDisposablePartFileSystem disposablePartFileSystem in _disposablePartFileSystems)
        {
            disposablePartFileSystem.Dispose();
        }
    }
}