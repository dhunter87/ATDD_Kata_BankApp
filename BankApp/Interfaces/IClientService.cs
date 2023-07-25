using System;
using BankApp.Models;

namespace BankApp.Interfaces
{
    public interface IClientService
    {
        IAccount? GetExistingAccount(string userName, string password);
        void OpenAccount(string firstname, string middleName, string sirname, string email, string dob, string password, string accountType);
    }
}