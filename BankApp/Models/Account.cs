using System;
using BankApp.Interfaces;

namespace BankApp.Models
{
	public class Account : IAccount
    {
		// todo replace with account type object
		private readonly string AccountType;
		private readonly string AccountId;
        private readonly string Name;
		private readonly string Dob;
		// todo replace with secure password feature
		private readonly string Password;

        public Account(string name, string dob, string password, string accountType)
		{
			AccountId = "Todo:ReplaceWithUniqueIdentifier";
			Name = name;
			Dob = dob;
			Password = password;
			AccountType = accountType;
		}
	}
}