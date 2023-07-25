using BankApp.Interfaces;

namespace BankApp.Boundaries
{
    public class ClientRepository : IClientRepository
    {
        public void CreateAccount(IAccount account)
        {
            throw new NotImplementedException();
        }

        public IAccount GetAccount(string accountType, string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}