using Core.Database.Models;

namespace Core.Helpers;

public class DataHandler
{
    public ClientData ClientData { get; set; } = new();
    public Device Device { get; set; } = new();
    public Token Token { get; set; } = new();
    public UsrAccount Account { get; set; } = new();
}