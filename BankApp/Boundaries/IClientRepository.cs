namespace BankApp.Interfaces
{
    public interface IClientRepository
    {
        void CreateAccount(IAccount account);
        IAccount GetAccount(string accountType, string userName, string password);
    }
}