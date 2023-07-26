using System;
namespace BankApp.Interfaces
{
    public interface IAccountRepository
    {
        double DepositFunds(IAccount userAccount, double amountToDeposit);
        double GetBalance(IAccount userAccount);
    }
}