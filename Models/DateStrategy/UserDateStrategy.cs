using Backups.Interfaces;

namespace Backups.Models.DateStrategy;

public class UserDateStrategy : IDateStrategy
{
    public DateTime UserDate { get; set; }

    public DateTime GetDate => UserDate;
}