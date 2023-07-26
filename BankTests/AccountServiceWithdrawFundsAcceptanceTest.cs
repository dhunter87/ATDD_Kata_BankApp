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

namespace BankTests
{
    [TestFixture]
    [Story(AsA = "a client of the bank",
           IWant = "to be able to withdraw my money from my account",
           SoThat = "I can access my money easily when I need to use it")]

    public class AccountServiceWithdrawFundsAcceptanceTest : AcceptanceTestBase
    {
        [Test]
        [Ignore("")]
        public void NewAccountDepositOf100GivesBalanceOf1000()
        {
            var startingBalance = 300;
            var withdrawalAmmount = 200;
            var expectedBalance = 100;

            this.Given((s) => s.AClientHasAnAccountWithBalanceOf(startingBalance))
                .When(s => s.TheClientMakesAWithdrawalOf(withdrawalAmmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
                .BDDfy();
        }

        private void TheClientMakesAWithdrawalOf(double balanceToWithdraw, double startingBalance)
        {
            if (BankAccountService != null)
            {
                BankAccountService.WithdrawFunds(UserAccount, balanceToWithdraw);
                MockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == UserAccount))).Returns(startingBalance + balanceToWithdraw);
            }
        }
    }
}
