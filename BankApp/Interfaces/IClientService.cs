using System;
using BankApp.Models;

namespace BankApp.Interfaces
{
    public interface IClientService
    {
        Account OpenAccount(string name, string dob, string password);
    }
}

