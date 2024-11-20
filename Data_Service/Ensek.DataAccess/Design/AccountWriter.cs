using Ensek.Database.Contexts;
using Ensek.Database.Tables;

using Microsoft.Extensions.Logging;

namespace Ensek.DataAccess.Design;

/// <summary>
/// AccountId,FirstName,LastName
/// </summary>
/// <remarks>
/// `FirstName` || `LastName` || (`FirstName` && `LastName`) Will NOT BE a unique key !
/// </remarks>
internal class AccountWriter : BaseWriter<EnsekContext, Account>, IAccountWriter
{
    private readonly EnsekContext _ensekContext;
    private readonly ILogger<BaseWriter<EnsekContext, Account>> _logger;

    public AccountWriter(EnsekContext ensekContext,
        ILogger<BaseWriter<EnsekContext, Account>> logger)
        : base(ensekContext, logger)
    {
        _ensekContext = ensekContext;
        _logger = logger;
    }

    public async Task<int> CreateAccountAsync(string firstName, string lastName)
    {
        _logger.LogTrace("CreateAccountAsync IN {0} {1}", firstName, lastName);
        var entity = new Account
        {
            FirstName = firstName,
            LastName = lastName
        };
        Account account = await CreateAsync(entity);

        _logger.LogTrace("CreateAccountAsync OUT ID: {0}", account.AccountId);
        return account.AccountId;
    }
}