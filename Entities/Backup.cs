using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _restorePoints;

    public Backup()
    {
        _restorePoints = new List<RestorePoint>();
    }

    public IEnumerable<RestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        _restorePoints.Add(restorePoint);
    }

    public void RemoveRestorePoint(RestorePoint restorePoint)
    {
        if (!_restorePoints.Contains(restorePoint))
        {
            throw BackupExceptions.BackupDoesNotContainRestorePoint();
        }

        _restorePoints.Remove(restorePoint);
    }
}