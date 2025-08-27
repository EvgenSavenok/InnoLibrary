namespace DBSeeder.Contracts;

public interface IBaseRepository 
{
    Task<bool> InsertRandomData();
}