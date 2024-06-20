using Database.Models;

namespace Core.Helpers;

public class DataHandler
{
    public ClientData ClientData { get; set; } = default!;
    public Device Device { get; set; } = default!;
    public Token Token { get; set; } = default!;
    public UsrAccount Account { get; set; } = default!;
}