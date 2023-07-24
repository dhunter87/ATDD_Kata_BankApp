using System;
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
            throw new NotImplementedException();
        }

        public IAccount OpenAccount(string name, string dob, string password, string accountType)
        {
            return new Account(name, dob, password, accountType);
        }
    }
}