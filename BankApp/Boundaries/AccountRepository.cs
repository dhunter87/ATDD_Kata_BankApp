using System;
using BankApp.Interfaces;

namespace BankApp.Boundaries
{
	public class AccountRepository : IAccountRepository
    {
		public AccountRepository()
		{
		}

        public double DepositFunds(IAccount userAccount)
        {
            throw new NotImplementedException();
        }

        public double GetBalance(IAccount userAccount)
        {
            throw new NotImplementedException();
        }
    }
}