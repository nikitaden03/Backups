using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Models;
using Backups.Models.DateStrategy;

namespace Backups.Entities;

public class BackupTask : IBackupTask
{
    private readonly IBackup _backup;
    private readonly IRepository _repository;
    private readonly IStorageAlgorithm _algorithm;
    private readonly List<BackupObject> _backupObjects;

    public BackupTask(IStorageAlgorithm algorithm, IRepository repository, IDateStrategy dateStrategy)
    {
        _algorithm = algorithm;
        _repository = repository;
        _backupObjects = new List<BackupObject>();
        _backup = new Backup();
        PathToBackupTask = Guid.NewGuid().ToString();
        _repository.CreateDirectory(PathToBackupTask);
        DateStrategy = dateStrategy;
    }

    public BackupTask(IStorageAlgorithm algorithm, IRepository repository)
        : this(algorithm, repository, new CurrentDateStrategy())
    {
    }

    public string PathToBackupTask { get; }

    public IBackup Backup => _backup;

    public IDateStrategy DateStrategy { get; set; }

    public void AddBackupObject(BackupObject backupObject)
    {
        if (_backupObjects.Contains(backupObject))
        {
            throw BackupTaskException.BackupTaskAlreadyContainBackupObject();
        }

        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(BackupObject backupObject)
    {
        if (!_backupObjects.Contains(backupObject))
        {
            throw BackupTaskException.BackupTaskDoesNotContainBackupObject();
        }

        _backupObjects.Remove(backupObject);
    }

    public RestorePoint CreateRestorePoint()
    {
        var restorePointId = Guid.NewGuid();
        _repository.CreateDirectory($@"{PathToBackupTask}{_repository.PathSeparator}{restorePointId}");
        var partOfFileSystems = _backupObjects.Select(backupObject => backupObject.Repository.OpenPartOfFileSystem(backupObject.RelativePath)).ToList();
        IStorage storage = _algorithm.CreateStorage(partOfFileSystems, $@"{PathToBackupTask}{_repository.PathSeparator}{restorePointId}", _repository);
        var restorePoint = new RestorePoint(_backupObjects, storage, DateStrategy.GetDate, restorePointId);
        _backup.AddRestorePoint(restorePoint);
        return restorePoint;
    }
}