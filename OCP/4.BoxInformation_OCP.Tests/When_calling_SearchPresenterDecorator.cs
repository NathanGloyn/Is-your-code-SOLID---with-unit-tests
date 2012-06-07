using System;
using BoxInformation.Interfaces;
using BoxInformation.PresenterDecorators;
using NSubstitute;
using NUnit.Framework;

namespace _4.BoxInformation_OCP.Tests
{
    [TestFixture]
    public class When_calling_SearchPresenterDecorator
    {
        private ISearchPresenter fakePresenter;
        private ILogger fakeLogger;

        [SetUp]
        public void Setup()
        {
            fakePresenter = Substitute.For<ISearchPresenter>();
            fakeLogger = Substitute.For<ILogger>();
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_no_presenter()
        {
            Assert.Throws<ArgumentNullException>(() => new SearchPresenterDecorator(null, fakeLogger));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_no_logger()
        {
            Assert.Throws<ArgumentNullException>(() => new SearchPresenterDecorator(fakePresenter, null));
        }

        [Test]
        public void Should_log_GetSearchResults_was_called()
        {
            var target = new SearchPresenterDecorator(fakePresenter, fakeLogger);

            target.GetSearchResults();

            fakeLogger.Received().Log(Arg.Any<string>());
        }

         
    }
}