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

    public class AccountServiceDisplayBalanceAcceptanceTest : AcceptanceTestBase
    {
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

        private double TheClientViewsTheirBalance(double mockGetBalanceValue)
        {
            if (BankClientService != null && UserAccount != null && BankAccountService != null)
            {
                return BankAccountService.GetBalance(UserAccount);
            }

            return 0;
        }
    }
}
