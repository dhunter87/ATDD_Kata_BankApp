using System;
using System.Security.Principal;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Services
{
	public class AccountService : IAccountService
    {
		public AccountService()
		{
		}

        public int GetBalance(Account userAccount)
        {
            if (userAccount == null)
            {
                // Handle the case when the account is null (you can throw an exception or return a default balance)
                // For example, you can throw an InvalidOperationException or return a default balance of 0.
                throw new InvalidOperationException("Account cannot be null.");
            }
            throw new NotImplementedException();
        }
    }
}

