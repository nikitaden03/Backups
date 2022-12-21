namespace Backups.Interfaces;

public interface IDateStrategy
{
    DateTime GetDate { get; }
}