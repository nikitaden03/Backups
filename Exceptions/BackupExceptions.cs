namespace Backups.Exceptions;

public class BackupExceptions : Exception
{
    private BackupExceptions(string message)
        : base(message)
    {
    }

    public static BackupExceptions BackupDoesNotContainRestorePoint()
    {
        return new BackupExceptions("Backup does not contain restore point!");
    }
}