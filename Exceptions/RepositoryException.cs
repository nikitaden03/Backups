namespace Backups.Exceptions;

public class RepositoryException : Exception
{
    private RepositoryException(string message)
        : base(message)
    {
    }

    public static RepositoryException FileIsNotInRepository()
    {
        return new RepositoryException("The requested file is located outside the repository");
    }

    public static RepositoryException FileDoesNotExist()
    {
        return new RepositoryException("The requested file does not exist");
    }

    public static RepositoryException FileAlreadyExist()
    {
        return new RepositoryException("Attempt to create an existing file");
    }

    public static RepositoryException DirectoryAlreadyExist()
    {
        return new RepositoryException("Attempt to create an existing directory");
    }
}