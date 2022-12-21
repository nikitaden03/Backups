namespace Backups.Exceptions;

public class BackupTaskException : Exception
{
    private BackupTaskException(string message)
        : base(message)
    {
    }

    public static BackupTaskException BackupTaskDoesNotContainBackupObject()
    {
        return new BackupTaskException("Backup task does not contain backup object");
    }

    public static BackupTaskException BackupTaskAlreadyContainBackupObject()
    {
        return new BackupTaskException("Backup task already contains backup object");
    }
}