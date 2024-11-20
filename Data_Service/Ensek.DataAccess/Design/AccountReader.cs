using Ensek.Database.Contexts;
using Ensek.Database.Tables;

using Microsoft.EntityFrameworkCore;

namespace Ensek.DataAccess.Design;

/// <summary>
/// AccountId,FirstName,LastName
/// </summary>
/// <remarks>
/// `FirstName` || `LastName` || (`FirstName` && `LastName`) Will NOT BE a unique key !
/// </remarks>
internal class AccountReader : IAccountReader
{
    private readonly EnsekContext _ensekContext;

    public AccountReader(EnsekContext ensekContext)
    {
        _ensekContext = ensekContext;
    }

    public Task<Account?> GetAccountAsync(int accountId)
    {
        return _ensekContext.Accounts.SingleOrDefaultAsync(a => a.AccountId == accountId);
    }
}