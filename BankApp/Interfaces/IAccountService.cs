using BankApp.Models;

namespace BankApp.Interfaces
{
    public interface IAccountService
    {
        int DepositFunds(IAccount? userAccount, int balanceToDeposit);
        int GetBalance(IAccount userAccount);
    }
}