using System;

namespace Core.Interfaces
{
    public interface ICredAdder
    {
        public void AddCredentials(string ClientId, string ClientSecret);
    }
}
