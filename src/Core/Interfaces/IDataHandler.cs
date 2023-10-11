using System;
using Core.Data;

namespace Core.Interfaces
{
    public interface IDataHandler
    {
        Task SaveAsync();

        public ClientData data { get; }

        public UsrAccount account { get; }
        public Token Token { get; }
        public Device Device { get; set; }
    }
}
