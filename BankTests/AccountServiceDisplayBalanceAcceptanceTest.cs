using Microsoft.Extensions.DependencyInjection;
using System.Security.Principal;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;
using TestStack.BDDfy.Core;
using TestStack.BDDfy;
using NUnit.Framework;
using Moq;
using BankApp.DependencyInjection;
using BankApp.Interfaces;
using BankApp.Services;
using BankApp.Models;
using BankTests.Constants;

namespace BankTests
{
    [TestFixture]
    [Story(AsA = "a client of the bank",
           IWant = "to be able to view my account balance",
           SoThat = "I can know how much money is in my account")]

    public class AccountServiceDisplayBalanceAcceptanceTest
    {
        private readonly IServiceProvider _serviceProvider;

        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IAccountRepository> _mockAccountRepository;

        private IAccountService? _accountService;
        private IClientService? _clientService;
        private IAccount? _userAccount;

        public int Balance { get; set; }

        public AccountServiceDisplayBalanceAcceptanceTest()
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

            var testAccount = new Account(
                TestConstants.FirstName,
                TestConstants.MiddleName,
                TestConstants.SirName,
                TestConstants.Email,
                TestConstants.DateOfBirth,
                TestConstants.Password,
                TestConstants.AccountType);

            _mockClientRepository.Setup(cr => cr.GetAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(testAccount);
        }

        [Test]
        public void NewAccountZeroBalance()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientViewsTheirBalance(0))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(0))
                .BDDfy();
        }

        [Test]
        public void ExistingAccountWithBalanceOf1000()
        {
            this.Given((s) => s.AClientHasAnAccountWithBalanceOf(1000))
                .When(s => s.TheClientViewsTheirBalance(1000))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(1000))
                .BDDfy();
        }

        private void AClientOpensANewAccount()
        {
            if (_clientService != null)
            {
                _clientService.OpenAccount(
                    TestConstants.FirstName,
                    TestConstants.MiddleName,
                    TestConstants.SirName,
                    TestConstants.Email,
                    TestConstants.DateOfBirth,
                    TestConstants.Password,
                    TestConstants.AccountType);

                _userAccount = _clientService.GetExistingAccount(TestConstants.UserName, TestConstants.Password);

                return;
            }
            Assert.Fail();
        }

        private void AClientHasAnAccountWithBalanceOf(int startingBalance)
        {
            _mockAccountRepository.Setup(ar => ar.GetBalance(It.IsAny<IAccount>())).Returns(startingBalance);

            if (_clientService != null &&
                _accountService != null)
            {
                _userAccount = _clientService.GetExistingAccount(TestConstants.UserName, TestConstants.Password);

                if (_userAccount != null)
                {
                    Balance = _accountService.GetBalance(_userAccount);
                }
            }

            Assert.That(Balance, Is.EqualTo(startingBalance));
        }

        private void TheClientViewsTheirBalance(int mockGetBalanceValue)
        {
            _mockAccountRepository.Setup(mr => mr.GetBalance(It.IsAny<IAccount>())).Returns(mockGetBalanceValue);

            if (_userAccount != null && _accountService != null)
            {
                Balance = _accountService.GetBalance(_userAccount);
                return;
            }
            Assert.Fail();
        }

        private void TheClientShouldHaveAnAccountBalanceOf(int expectedBalance)
        {
            Assert.That(Balance, Is.EqualTo(expectedBalance));
        }
    }
}
