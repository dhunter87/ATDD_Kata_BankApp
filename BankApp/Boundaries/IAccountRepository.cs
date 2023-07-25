using System;
namespace BankApp.Interfaces
{
    public interface IAccountRepository
    {
        int DepositFunds(IAccount userAccount);
        int GetBalance(IAccount userAccount);
    }
}