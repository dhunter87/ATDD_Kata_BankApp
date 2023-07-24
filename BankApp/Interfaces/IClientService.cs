using System;
using BankApp.Models;

namespace BankApp.Interfaces
{
    public interface IClientService
    {
        IAccount? GetExistingAccount(string userName, string password);
        IAccount OpenAccount(string name, string dob, string password, string accountType);
    }
}