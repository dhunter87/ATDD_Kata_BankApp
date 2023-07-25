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
using System.Xml.Linq;
using BankTests.Constants;

namespace BankTests
{
    [TestFixture]
    [Story(AsA = "a client of the bank",
           IWant = "to be able to deposit my money",
           SoThat = "I can store my money safely when I am not using it")]

    public class AccountServiceAcceptanceTest
    {
        private readonly IServiceProvider _serviceProvider;

        private Mock<IClientRepository> _mockClientRepository;
        private Mock<IAccountRepository> _mockAccountRepository;
        private IAccountService? _accountService;
        private IClientService? _clientService;
        private IAccount? _userAccount;
        private Mock<IAccount> _mockUserAccount;

        public int Balance { get; set; }

        public AccountServiceAcceptanceTest()
        {
            _mockUserAccount = new Mock<IAccount>();
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
        public void NewAccountDepositOf100GivesBalanceOf1000()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(1000, 0))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(1000))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOf2000GivesBalanceOf2000()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(2000, 0))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(2000))
                .BDDfy();
        }

        [Test]
        public void ExistingAccountwithBalanceOf500DepositOf150GivesBalanceOf650()
        {
            this.Given((s) => s.AClientHasAnAccountWithBalanceOf(500))
                .When(s => s.TheClientMakesADepositOf(150, 500))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(650))
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

        private void TheClientMakesADepositOf(int balanceToDeposit, int startingBalance)
        {
            if (_accountService != null)
            {
                _accountService.DepositFunds(_userAccount, balanceToDeposit);
                _mockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == _userAccount))).Returns(startingBalance + balanceToDeposit);
            }
        }

        private void TheClientShouldHaveAnAccountBalanceOf(int expectedBalance)
        {
            if (_userAccount != null && _accountService != null)
            {
                Balance = _accountService.GetBalance(_userAccount);
                Assert.That(Balance, Is.EqualTo(expectedBalance));
                return;
            }

            Assert.Fail();
        }
    }
}
