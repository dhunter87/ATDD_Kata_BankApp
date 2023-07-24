using BankApp.DependencyInjection;

namespace BankApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DependencyInjectionProvider.Setup();
        }
    }
}
