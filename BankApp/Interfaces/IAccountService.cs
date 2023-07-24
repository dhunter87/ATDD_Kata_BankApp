using BankApp.Models;

namespace BankApp.Interfaces
{
    public interface IAccountService
    {
        int GetBalance(IAccount userAccount);
    }
}