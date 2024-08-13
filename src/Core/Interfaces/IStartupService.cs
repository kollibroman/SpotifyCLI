namespace Core.Interfaces;

public interface IStartupService
{
    public Task LoadDatabaseDataASync();
}