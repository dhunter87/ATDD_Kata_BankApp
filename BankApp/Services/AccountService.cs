using System;
using System.Security.Principal;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Services
{
	public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository repository)
		{
            _accountRepository = repository;
		}

        public double DepositFunds(IAccount? userAccount, double balanceToDeposit)
        {
            if (userAccount != null)
            {
                return _accountRepository.DepositFunds(userAccount, balanceToDeposit);
            }
            throw new ArgumentException();
        }

        public double GetBalance(IAccount userAccount)
        {
            if (userAccount != null)
            {
                try
                {
                    return _accountRepository.GetBalance(userAccount);
                }
                catch (Exception ex)
                {
                    // todo create custom AccountNotFoundException
                    throw new ArgumentException(ex.ToString());
                }
            }

            throw new InvalidOperationException("Account cannot be null.");
        }

        public double WithdrawFunds(IAccount? userAccount, double balanceToWithdraw)
        {
            if (userAccount != null)
            {
                return _accountRepository.WithdrawFunds(userAccount, balanceToWithdraw);
            }
            throw new ArgumentException();
        }
    }
}