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
using static System.Runtime.InteropServices.JavaScript.JSType;
using BankApp.Exceptions;

namespace BankTests
{ 
    [TestFixture]
    [Story(AsA = "a client of the bank",
           IWant = "to be able to deposit my money",
           SoThat = "I can store my money safely when I am not using it")]

    public class AccountServiceDepositFundsAcceptanceTest : AcceptanceTestBase
    {
        [Test]
        public void NewAccountDepositOf1000GivesBalanceOf1000()
        {
            var startingBalance = 0;
            var depositAmount = 1000;
            var expectedBalance = 1000;

            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(depositAmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOf2000GivesBalanceOf2000()
        {
            var startingBalance = 0;
            var depositAmount = 2000;
            var expectedBalance = 2000;

            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(depositAmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
                .BDDfy();
        }

        [Test]
        public void ExistingAccountwithBalanceOf500DepositOf150GivesBalanceOf650()
        {
            var startingBalance = 500;
            var depositAmount = 150;
            var expectedBalance = 650;

            this.Given((s) => s.AClientHasAnAccountWithBalanceOf(startingBalance))
                .When(s => s.TheClientMakesADepositOf(depositAmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOf500GAndDepositOf200GivesBalanceOf700()
        {
            int startingBalance = 0;
            int firstDepositAmount = 500;
            int secondDepositAmount = 200;
            int expectedFinalBalance = 700;

            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(firstDepositAmount, startingBalance))
                .And(s => s.TheClientMakesADepositOf(secondDepositAmount, firstDepositAmount))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedFinalBalance))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOfDecmalValueGivesBalanceOfDecimalValue()
        {
            var depositAmmount = 0.5;
            int startingBalance = 0;

            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(depositAmmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(depositAmmount))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOfover2BillionGivesBalanceOfOver2Billion()
        {
            var depositAmmount = 2150000000;
            int startingBalance = 0;

            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(depositAmmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(depositAmmount))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOfover3BillionAndOneIsRejected()
        {
            var depositAmmount = 3000000001;
            int startingBalance = 0;

            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientAttemptsToMakeADepositOf(depositAmmount, startingBalance))
                .Then(s => s.TheDepositShouldBeRejected(depositAmmount))
                .And(s => s.TheClientShouldBeToldDepostExceedsMaximumAccountBalance())
                .BDDfy();
        }
        private void TheClientAttemptsToMakeADepositOf(double depositAmount, double startingBalance)
        {
            var maxAccountBalane = 3000000000;
            if (BankAccountService != null && UserAccount != null)
            {
                if (startingBalance + depositAmount > maxAccountBalane)
                {
                    MockAccountRepository.Setup(a => a.DepositFunds(UserAccount, depositAmount))
                             .Throws(new CustomException("Some error happened"));
                }
                else
                {
                    MockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == UserAccount))).Returns(startingBalance + depositAmount);
                }
            }
        }

        private void TheDepositShouldBeRejected(double amountToDeposit)
        {
            if (UserAccount != null && BankAccountService != null)
            {
                try
                {
                    BankAccountService.DepositFunds(UserAccount, amountToDeposit);
                }
                catch (CustomException ex)
                {
                    ScenarioException = ex;
                }
            }
        }

        private void TheClientShouldBeToldDepostExceedsMaximumAccountBalance()
        {
            if (ScenarioException == null)
            {
                Assert.Fail();
            }

            Assert.That(ScenarioException?.Message, Is.EqualTo("Some error happened"));
        }

        private void TheClientMakesADepositOf(double depositAmount, double startingBalance)
        {
            if (BankAccountService != null)
            {
                MockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == UserAccount))).Returns(startingBalance + depositAmount);
                BankAccountService.DepositFunds(UserAccount, depositAmount);
            }
        }
    }
}
