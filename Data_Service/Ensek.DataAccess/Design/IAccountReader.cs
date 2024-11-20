using Ensek.Database.Tables;

namespace Ensek.DataAccess.Design;

public interface IAccountReader
{
    Task<Account?> GetAccountAsync(int accountId);
}