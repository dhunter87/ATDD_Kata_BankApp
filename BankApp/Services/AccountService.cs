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

        public int GetBalance(IAccount userAccount)
        {
            if (userAccount != null)
            {
                return 0;
            }

            throw new InvalidOperationException("Account cannot be null.");
        }
    }
}