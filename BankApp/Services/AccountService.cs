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

        public int DepositFunds(IAccount? userAccount, int balanceToDeposit)
        {
            if (userAccount != null)
            {
                return 0;
                //return _accountRepository.DepositFunds(userAccount);
            }
            return -1;
        }

        public int GetBalance(IAccount userAccount)
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
    }
}