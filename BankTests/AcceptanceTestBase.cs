﻿using System;
using System.Security.Principal;
using BankApp.DependencyInjection;
using BankApp.Exceptions;
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
        protected Exception? ScenarioException;
        protected int Balance { get; set; }


        [OneTimeSetUp]
        public void SetUpFixture()
        {
            ServiceProvider = DependencyInjectionProvider.Setup(sc =>
            {
                sc.AddTransient<IClientRepository>(_ => MockClientRepository.Object);
                sc.AddTransient<IAccountRepository>(_ => MockAccountRepository.Object);
                sc.AddTransient<IAccountService, AccountService>();
                sc.AddTransient<IClientService, ClientService>();
            });
        }

        [SetUp]
        public void SetUpTestCase()
        {
            MockClientRepository = new Mock<IClientRepository>();
            MockAccountRepository = new Mock<IAccountRepository>();

            BankAccountService = ServiceProvider.GetService<IAccountService>()!;
            BankClientService = ServiceProvider.GetService<IClientService>()!;
            UserAccount = null; 
            ScenarioException = null; 

            SetupMockClientRepositoryGetAccount();
        }

        public IAccount? AClientOpensANewAccount()
        {
            if (BankClientService != null)
            {
                BankClientService.OpenAccount(TestConstants.FirstName, TestConstants.MiddleName, TestConstants.SirName,
                                              TestConstants.Email, TestConstants.DateOfBirth, TestConstants.Password, TestConstants.AccountType);

                return BankClientService.GetExistingAccount(TestConstants.UserName, TestConstants.Password);
            }

            throw new AggregateException();
        }

        public double AClientHasAnAccountWithBalanceOf(double startingBalance)
        {
            double balance = 0;
            if (UserAccount != null)
            {
                MockAccountRepository.Setup(ar => ar.GetBalance(UserAccount)).Returns(startingBalance);
            }

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