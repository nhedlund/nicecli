namespace NiceCli;

public class CliUserException : Exception
{
  public CliUserException()
  {
  }

  public CliUserException(string message) : base(message)
  {
  }

  public CliUserException(string message, Exception inner) : base(message, inner)
  {
  }
}
