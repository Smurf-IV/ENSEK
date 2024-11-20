using System.Threading.Tasks;

namespace Ensek.Service.Services;

public interface IAccountService
{
    Task<bool> CheckAccountIdAsync(ulong tsId, int accountId);
}