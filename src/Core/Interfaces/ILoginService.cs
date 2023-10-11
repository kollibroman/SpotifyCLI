using System;

namespace Core.Interfaces
{
    public interface ILoginService
    {
        Task Login();
        Task Logout();
    }
}
