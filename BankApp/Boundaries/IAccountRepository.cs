using System;
namespace BankApp.Interfaces
{
    public interface IAccountRepository
    {
        double DepositFunds(IAccount userAccount, double amountToDeposit);
        double GetBalance(IAccount userAccount);
        double WithdrawFunds(IAccount userAccount, object balanceToDeposit);
    }
}