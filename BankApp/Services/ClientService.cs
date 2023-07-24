using System;
using BankApp.Interfaces;
using BankApp.Models;

namespace BankApp.Services
{
	public class ClientService : IClientService
    {
		public ClientService()
		{
		}

        public Account OpenAccount(string name, string dob, string password)
        {
            throw new NotImplementedException();
        }
    }
}

