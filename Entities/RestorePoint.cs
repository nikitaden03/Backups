using Backups.Interfaces;
using Backups.Models;

namespace Backups.Entities;

public class RestorePoint
{
    private List<BackupObject> _backupObjects;

    public RestorePoint(IEnumerable<BackupObject> backupObjects, IStorage storage, DateTime dateTime, Guid id)
    {
        _backupObjects = backupObjects.ToList();
        Storage = storage;
        Date = dateTime;
        Id = id;
    }

    public Guid Id { get; }
    public DateTime Date { get; }
    public IStorage Storage { get; }
    public IEnumerable<BackupObject> BackupObjects => _backupObjects;
}