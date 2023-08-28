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
           IWant = "to be able to withdraw my money from my account",
           SoThat = "I can access my money easily when I need to use it")]

    public class AccountServiceWithdrawFundsAcceptanceTest : AcceptanceTestBase
    {
        [Test]
        public void ExistingAccountBalance300WithdrawalOf200GivesBalanceOf100()
        {
            var startingBalance = 300;
            var withdrawalAmmount = 200;
            var expectedBalance = 100;

            this.Given((s) => s.AClientHasAnAccountWithBalanceOf(startingBalance))
                .When(s => s.TheClientMakesAWithdrawalOf(withdrawalAmmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
                .BDDfy();
        }

        [Test]
        public void ExistingAccountBalance100WithdrawalOf200GivesBalanceOfMinus100()
        {
            var startingBalance = 100;
            var withdrawalAmmount = 200;
            var expectedBalance = -100;

            this.Given((s) => s.AClientHasAnAccountWithBalanceOf(startingBalance))
                .When(s => s.TheClientMakesAWithdrawalOf(withdrawalAmmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
                .BDDfy();
        }

        [Test]
        public void NewAccountWithdrawalOf1001GivesErrorCannotExceedOverdraftOf1000()
        {
            var withdrawalAmmount = 1001;
            var expectedBalance = 0;

            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientAttemptsToMakeAWithdrawalOf(withdrawalAmmount, expectedBalance))
                .Then(s => s.TheWithdrawalShouldBeRejected(withdrawalAmmount))
                .And(s => s.TheClientShouldBeToldWithdrawalExceedsOverdraftLimitsAccountBalance())
                .BDDfy();
        }

        private void TheClientMakesAWithdrawalOf(double balanceToWithdraw, double startingBalance)
        {
            if (BankAccountService != null)
            {
                MockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == UserAccount))).Returns(startingBalance - balanceToWithdraw);
                BankAccountService.WithdrawFunds(UserAccount, balanceToWithdraw);
            }
        }

        private void TheClientAttemptsToMakeAWithdrawalOf(double balanceToWithdraw, double startingBalance)
        {
            var maxOverdraftLimit = -1000;
            if (UserAccount != null && BankAccountService != null)
            {
                if (startingBalance - balanceToWithdraw < maxOverdraftLimit)
                {
                    MockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == UserAccount))).Returns(0);
                    MockAccountRepository.Setup(a => a.WithdrawFunds(UserAccount, balanceToWithdraw)).Throws(new CustomException("Some error happened"));
                }
            }
        }

        private void TheWithdrawalShouldBeRejected(double amountToWithdraw)
        {
            if (UserAccount != null && BankAccountService != null)
            {
                try
                {
                    BankAccountService.WithdrawFunds(UserAccount, amountToWithdraw);
                }
                catch (CustomException ex)
                {
                    ScenarioException = ex;
                }
            }
        }

        private void TheClientShouldBeToldWithdrawalExceedsOverdraftLimitsAccountBalance()
        {
            if (ScenarioException == null)
            {
                Assert.Fail();
            }

            Assert.That(ScenarioException?.Message.Contains("Some error happened"), Is.True);
        }
    }
}