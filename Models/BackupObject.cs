using Backups.Interfaces;

namespace Backups.Models;

public class BackupObject
{
    public BackupObject(string relativePath, IRepository repository)
    {
        RelativePath = relativePath;
        Id = Guid.NewGuid();
        Repository = repository;
    }

    public string RelativePath { get; }

    public Guid Id { get; }

    public IRepository Repository { get; }
}