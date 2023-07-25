using System;
namespace BankApp.Interfaces
{
    public interface IAccountRepository
    {
        double DepositFunds(IAccount userAccount);
        double GetBalance(IAccount userAccount);
    }
}