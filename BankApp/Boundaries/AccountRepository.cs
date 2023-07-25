using System;
using BankApp.Interfaces;

namespace BankApp.Boundaries
{
	public class AccountRepository : IAccountRepository
    {
		public AccountRepository()
		{
		}

        public int DepositFunds(IAccount userAccount)
        {
            throw new NotImplementedException();
        }

        public int GetBalance(IAccount userAccount)
        {
            throw new NotImplementedException();
        }
    }
}