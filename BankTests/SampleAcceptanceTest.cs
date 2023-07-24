using BankApp.DependencyInjection;
using BankApp.SampleBoundary;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using TestStack.BDDfy;
using TestStack.BDDfy.Core;
using TestStack.BDDfy.Scanners.StepScanners.Fluent;

namespace BankTests
{
    [TestFixture]
    [Story(
        SoThat = "Something great happens",
        AsA = "user of the system",
        IWant = "to do something that initiates that something great")]
    public class SampleAcceptanceTest
    {
        private readonly Mock<IBoundary> _boundaryTestDouble = new Mock<IBoundary>();
        private readonly IServiceProvider _serviceProvider;

        public SampleAcceptanceTest()
        {
            _serviceProvider = DependencyInjectionProvider.Setup(sc =>
            {
                // add your overrides of your boundary interfaces here
                sc.AddTransient<IBoundary>(sc1 => _boundaryTestDouble.Object);
            });

            // resolve an object from the dependency injection container providing the services
            // var sut = _serviceProvider.GetService<MyTargetObectIWantToTest>();
        }

        [Test]
        public void AcceptanceTestOne()
        {
            this.Given((s) => s.TheGivenStep())
                .When(s => s.TheWhenStep())
                .Then(s => s.TheThenStep())
                .BDDfy();
        }

        private void TheGivenStep()
        {
            // an arrange step
        }

        private void TheWhenStep()
        {
            // the act step.  
        }

        private void TheThenStep()
        {
            // an assert step
            Assert.Pass();
        }
    }
}
