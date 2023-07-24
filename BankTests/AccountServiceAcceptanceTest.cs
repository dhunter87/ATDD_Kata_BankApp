using BankApp.DependencyInjection;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using TestStack.BDDfy;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;
using BankApp.Interfaces;
using System.Security.Principal;
using BankApp.Models;
using BankApp.Services;

namespace BankTests
{
    [TestFixture]
    [Story(AsA = "a client of the bank",
           IWant = "to be able to view my account balance",
           SoThat = "I can know how much money is in my account")]

    public class AccountServiceAcceptanceTest
    {
        private readonly IServiceProvider _serviceProvider;

        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IAccountRepository> _mockAccountRepository;
        private IAccountService? _accountService;
        private IClientService? _clientService;
        private Account? _userAccount;

        public int Balance { get; set; }

        public AccountServiceAcceptanceTest()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();

            _serviceProvider = DependencyInjectionProvider.Setup(sc =>
            {
                sc.AddTransient<IClientRepository>(_ => _mockClientRepository.Object);
                sc.AddTransient<IAccountRepository>(_ => _mockAccountRepository.Object);
                sc.AddTransient<IAccountService, AccountService>();
                sc.AddTransient<IClientService, ClientService>();
            });

            _clientService = _serviceProvider.GetService<IClientService>()!;
            _accountService = _serviceProvider.GetService<IAccountService>()!;
        }

        [Test]
        public void AcceptanceTestOne()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientViewsTheirBalance())
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOfZero())
                .BDDfy();
        }

        private void AClientOpensANewAccount()
        {
            var name = "";
            var dateOfBirth = "";
            var password = "";

            _userAccount = _clientService?.OpenAccount(name, dateOfBirth, password);
        }

        private void TheClientViewsTheirBalance()
        {
            if (_userAccount != null && _accountService != null)
            {
                Balance = _accountService.GetBalance(_userAccount);
            }
        }

        private void TheClientShouldHaveAnAccountBalanceOfZero()
        {
            Assert.That(Balance, Is.EqualTo(0));
        }
    }
}
