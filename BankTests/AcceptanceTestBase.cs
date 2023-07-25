using System;
using BankApp.DependencyInjection;
using BankApp.Interfaces;
using BankApp.Models;
using BankApp.Services;
using BankTests.Constants;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace BankTests
{
	public abstract class AcceptanceTestBase
	{
        protected IServiceProvider ServiceProvider;
        protected Mock<IClientRepository> MockClientRepository;
        protected Mock<IAccountRepository> MockAccountRepository;
        protected IAccountService? BankAccountService;
        protected IClientService? BankClientService;
        protected IAccount? UserAccount;
        protected int Balance { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockClientRepository = new Mock<IClientRepository>();
            MockAccountRepository = new Mock<IAccountRepository>();

            ServiceProvider = DependencyInjectionProvider.Setup(sc =>
            {
                sc.AddTransient<IClientRepository>(_ => MockClientRepository.Object);
                sc.AddTransient<IAccountRepository>(_ => MockAccountRepository.Object);
                sc.AddTransient<IAccountService, AccountService>();
                sc.AddTransient<IClientService, ClientService>();
            });

            BankClientService = ServiceProvider.GetService<IClientService>()!;
            BankAccountService = ServiceProvider.GetService<IAccountService>()!;

            SetupMockClientRepositoryGetAccount();
        }

        public IAccount? AClientOpensANewAccount()
        {
            if (BankClientService != null)
            { 
                return BankClientService.GetExistingAccount(TestConstants.UserName, TestConstants.Password);
            }

            throw new AggregateException();
        }

        public double AClientHasAnAccountWithBalanceOf(double startingBalance)
        {
            MockAccountRepository.Setup(ar => ar.GetBalance(It.IsAny<IAccount>())).Returns(startingBalance);
            double balance = 0;

            if (BankClientService != null && BankAccountService != null)
            {
                UserAccount = BankClientService.GetExistingAccount(TestConstants.UserName, TestConstants.Password);

                if (UserAccount != null)
                {
                    balance = BankAccountService.GetBalance(UserAccount);
                }
            }

            Assert.That(balance, Is.EqualTo(startingBalance));

            return balance;
        }

        public double TheClientShouldHaveAnAccountBalanceOf(double expectedBalance)
        {
            if (UserAccount != null && BankAccountService != null)
            {
                var balance = BankAccountService.GetBalance(UserAccount);
                Assert.That(balance, Is.EqualTo(expectedBalance));
                return balance;
            }
            return 0;
        }

        private void SetupMockClientRepositoryGetAccount()
        {
            UserAccount = new Account(
                TestConstants.FirstName,
                TestConstants.MiddleName,
                TestConstants.SirName,
                TestConstants.Email,
                TestConstants.DateOfBirth,
                TestConstants.Password,
                TestConstants.AccountType);

            MockClientRepository.Setup(cr => cr.GetAccount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(UserAccount);
        }
    }
}

