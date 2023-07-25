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
           IWant = "to be able to deposit my money",
           SoThat = "I can store my money safely when I am not using it")]

    public class AccountServiceAcceptanceTest : AcceptanceTestBase
    {
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

        [Test]
        public void NewAccountDepositOf500GAndDepositOf200GivesBalanceOf700()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(500, 0))
                .And(s => s.TheClientMakesADepositOf(200, 500))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(700))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOfDecmalValueGivesBalanceOfDecimalValue()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(0.5, 0))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(0.5))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOfover2BillionGivesBalanceOfOver2Billion()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(2150000000, 0))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(2150000000))
                .BDDfy();
        }

        [Test]
        public void NewAccountDepositOfover2BillionGivesBalanceOfOver2Million()
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(3000000001, 0))
                .Then(s => s.TheDepositShouldBeRejected(3000000000))
                .And(s => s.TheClientShouldBeToldDepostExceedsMaximumAccountBalance(3000000000))
                .BDDfy();
        }

        private void TheClientShouldBeToldDepostExceedsMaximumAccountBalance(double v)
        {
            throw new NotImplementedException();
        }

        private void TheDepositShouldBeRejected(double v)
        {
            throw new NotImplementedException();
        }

        private void TheClientMakesADepositOf(double balanceToDeposit, double startingBalance)
        {
            if (BankAccountService != null)
            {
                BankAccountService.DepositFunds(UserAccount, balanceToDeposit);
                MockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == UserAccount))).Returns(startingBalance + balanceToDeposit);
            }
        }
    }
}
