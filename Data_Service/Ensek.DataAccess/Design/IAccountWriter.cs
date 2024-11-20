namespace Ensek.DataAccess.Design;

public interface IAccountWriter
{
    /// <summary>
    /// Returns the AccountId
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <returns></returns>
    Task<int> CreateAccountAsync(string firstName, string lastName);
}