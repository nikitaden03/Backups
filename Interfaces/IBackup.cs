using Backups.Entities;

namespace Backups.Interfaces;

public interface IBackup
{
    IEnumerable<RestorePoint> RestorePoints { get; }

    void AddRestorePoint(RestorePoint restorePoint);

    void RemoveRestorePoint(RestorePoint restorePoint);
}