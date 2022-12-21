using Backups.Entities;
using Backups.Models;

namespace Backups.Interfaces;

public interface IBackupTask
{
    string PathToBackupTask { get; }

    IBackup Backup { get; }

    void AddBackupObject(BackupObject backupObject);

    void RemoveBackupObject(BackupObject backupObject);

    RestorePoint CreateRestorePoint();
}