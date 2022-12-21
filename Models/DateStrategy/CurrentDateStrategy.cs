using Backups.Interfaces;

namespace Backups.Models.DateStrategy;

public class CurrentDateStrategy : IDateStrategy
{
    public DateTime GetDate => DateTime.Now;
}