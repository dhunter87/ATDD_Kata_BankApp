using System;
using BankApp.Interfaces;

namespace BankApp.Models
{
	public class Account : IAccount
    {
		// todo replace with account type object
		private readonly string AccountType;
		private readonly Guid AccountId;
        private readonly string FirstName;
        private readonly string MiddleName;
        private readonly string SirName;
        private readonly string UserName;
        private readonly string Email;
        private readonly string Dob;
		// todo replace with secure password feature
		private readonly string Password;

        public Account(string firstname, string middleName, string sirname, string email, string dob, string password, string accountType)
		{
			AccountId = Guid.NewGuid();
			FirstName = firstname;
			MiddleName = middleName;
			SirName = sirname;
			Email = email;
			Dob = dob;
			Password = password;
			AccountType = accountType;
			UserName = $"{firstname}-{sirname}";

            // Todo: change UserName property to something like:
            // $"{firstname.Substring(3)}-{sirname}-{Guid.NewGuid().ToString().Substring(6)}";
        }
    }
}