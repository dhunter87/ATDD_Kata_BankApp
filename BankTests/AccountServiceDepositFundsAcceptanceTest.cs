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
        [TestCase(0, 1000, 1000)]
        [TestCase(0, 2000, 2000)]
        [TestCase(500, 150, 650)]
        [TestCase(0.5, 0, 0.5)]
        public void NewAccountDepositOfXGivesBalanceOfY(double startingBalance, double depositAmount, double expectedBalance)
        {
            this.Given((s) => s.AClientOpensANewAccount())
                .When(s => s.TheClientMakesADepositOf(depositAmount, startingBalance))
                .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
                .BDDfy();
        }

        //[Test]
        //public void NewAccountDepositOf100GivesBalanceOf1000()
        //{
        //    var startingBalance = 0;
        //    var depositAmount = 1000;
        //    var expectedBalance = 1000;

        //    this.Given((s) => s.AClientOpensANewAccount())
        //        .When(s => s.TheClientMakesADepositOf(depositAmount, startingBalance))
        //        .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
        //        .BDDfy();
        //}

        //[Test]
        //public void NewAccountDepositOf2000GivesBalanceOf2000()
        //{
        //    var startingBalance = 0;
        //    var depositAmount = 2000;
        //    var expectedBalance = 2000;

        //    this.Given((s) => s.AClientOpensANewAccount())
        //        .When(s => s.TheClientMakesADepositOf(depositAmount, startingBalance))
        //        .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
        //        .BDDfy();
        //}

        //[Test]
        //public void ExistingAccountwithBalanceOf500DepositOf150GivesBalanceOf650()
        //{
        //    var startingBalance = 500;
        //    var depositAmount = 150;
        //    var expectedBalance = 650;

        //    this.Given((s) => s.AClientHasAnAccountWithBalanceOf(startingBalance))
        //        .When(s => s.TheClientMakesADepositOf(depositAmount, startingBalance))
        //        .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedBalance))
        //        .BDDfy();
        //}

        //[TestCase(0, 500, 200, 700)]
        //public void NewAccountDepositOf500GAndDepositOf200GivesBalanceOf700(int startingBalance, int firstDepositAmount, int secondDepositAmount, int expectedFinalBalance)
        //{
        //    this.Given((s) => s.AClientOpensANewAccount())
        //        .When(s => s.TheClientMakesADepositOf(firstDepositAmount, startingBalance))
        //        .And(s => s.TheClientMakesADepositOf(secondDepositAmount, firstDepositAmount))
        //        .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(expectedFinalBalance))
        //        .BDDfy();
        //}

        //[Test]
        //public void NewAccountDepositOfDecmalValueGivesBalanceOfDecimalValue()
        //{
        //    this.Given((s) => s.AClientOpensANewAccount())
        //        .When(s => s.TheClientMakesADepositOf(0.5, 0))
        //        .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(0.5))
        //        .BDDfy();
        //}

        //[Test]
        //public void NewAccountDepositOfover2BillionGivesBalanceOfOver2Billion()
        //{
        //    this.Given((s) => s.AClientOpensANewAccount())
        //        .When(s => s.TheClientMakesADepositOf(2150000000, 0))
        //        .Then(s => s.TheClientShouldHaveAnAccountBalanceOf(2150000000))
        //        .BDDfy();
        //}

        //[Test]
        //public void NewAccountDepositOfover2BillionGivesBalanceOfOver2Million()
        //{
        //    this.Given((s) => s.AClientOpensANewAccount())
        //        .When(s => s.TheClientMakesADepositOf(3000000001, 0))
        //        .Then(s => s.TheDepositShouldBeRejected(3000000000))
        //        .And(s => s.TheClientShouldBeToldDepostExceedsMaximumAccountBalance(3000000000))
        //        .BDDfy();
        //}

        private void TheDepositShouldBeRejected(double amountToDeposit)
        {
            var threeBillionAndOne = 3000000001;

            if (UserAccount != null && BankAccountService != null)
            {
                MockAccountRepository.Setup(a => a.DepositFunds(UserAccount, threeBillionAndOne))
                         .Throws(new CustomException("Some error happened"));

                try
                {
                    BankAccountService.DepositFunds(UserAccount, threeBillionAndOne);
                }
                catch (CustomException ex)
                {
                    ScenarioException = ex;
                }
            }
        }

        private void TheClientShouldBeToldDepostExceedsMaximumAccountBalance(double amountToDeposit)
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
                BankAccountService.DepositFunds(UserAccount, depositAmount);
                MockAccountRepository.Setup(mr => mr.GetBalance(It.Is<IAccount>(a => a == UserAccount))).Returns(startingBalance + depositAmount);
            }
        }
    }
}
