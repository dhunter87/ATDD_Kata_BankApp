using BankApp.Models;

namespace BankApp.Interfaces
{
    public interface IAccountService
    {
        double DepositFunds(IAccount? userAccount, double balanceToDeposit);
        double GetBalance(IAccount userAccount);
    }
}