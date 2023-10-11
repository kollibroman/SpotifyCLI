namespace Core.Exception;

public class NotLoggedInException : System.Exception
{
    public NotLoggedInException(string Message) : base(Message)
    {
        
    }
}