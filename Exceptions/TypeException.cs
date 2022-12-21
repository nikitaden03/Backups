namespace Backups.Exceptions;

public class TypeException : Exception
{
    private TypeException(string message)
        : base(message)
    {
    }

    public static TypeException PartFileSystemIsNotFile()
    {
        return new TypeException("IPartFileSystem is not IFilePartFileSystem");
    }
}