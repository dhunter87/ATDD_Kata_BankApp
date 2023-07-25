using System;
using System.Xml.Linq;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Services
{
	public class ClientService : IClientService
    {
        private IAccount? Account { get; set; }
        private IClientRepository ClientRepository;

		public ClientService(IClientRepository clientRepository)
		{
            ClientRepository = clientRepository;
		}

        public IAccount? GetExistingAccount(string userName, string password)
        {
            var accountType = "Current";

            Account = ClientRepository.GetAccount(accountType, userName, password);
            return Account;
        }

        public void OpenAccount(string firstname, string middleName, string sirname, string email, string dob, string password, string accountType)
        {
            Account = new Account(firstname, middleName, sirname, email, dob, password, accountType);
            ClientRepository.CreateAccount(Account);
        }
    }
}