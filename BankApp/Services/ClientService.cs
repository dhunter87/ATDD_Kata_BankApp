using System;
using System.Xml.Linq;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Services
{
	public class ClientService : IClientService
    {
        private IClientRepository ClientRepository;

		public ClientService(IClientRepository clientRepository)
		{
            ClientRepository = clientRepository;
		}

        public IAccount? GetExistingAccount(string userName, string password)
        {
            var accountType = "Current";

            var account = ClientRepository.GetAccount(accountType, userName, password);
            return account;
        }

        public void OpenAccount(string firstname, string middleName, string sirname, string email, string dob, string password, string accountType)
        {
            var account = new Account(firstname, middleName, sirname, email, dob, password, accountType);
            ClientRepository.CreateAccount(account);
        }
    }
}