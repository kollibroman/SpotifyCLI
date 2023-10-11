using System;

namespace Core.Interfaces
{
    public interface ICredAdder
    {
       Task AddCredentials(string ClientId, string ClientSecret);
    }
}
