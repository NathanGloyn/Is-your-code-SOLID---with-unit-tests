using System;
using BoxInformation.Interfaces;
using BoxInformation.PresenterDecorators;
using NSubstitute;
using NUnit.Framework;

namespace _4.BoxInformation_OCP.Tests
{
    [TestFixture]
    public class When_calling_RecordPresenterDecorator
    {
        private IRecordPresenter fakePresenter;
        private ILogger fakeLogger;
        private RecordPresenterDecorator target;

        [SetUp]
        public void Setup()
        {
            fakePresenter = Substitute.For<IRecordPresenter>();
            fakeLogger = Substitute.For<ILogger>();

            target = new RecordPresenterDecorator(fakePresenter, fakeLogger);

        }

        [Test]
        public void Should_throw_ArgumentNullException_if_no_presenter()
        {
            Assert.Throws<ArgumentNullException>(() => new RecordPresenterDecorator(null, fakeLogger));
        }

        [Test]
        public void Should_throw_ArgumentNullException_if_no_logger()
        {
            Assert.Throws<ArgumentNullException>(() => new RecordPresenterDecorator(fakePresenter, null));
        }

        [Test]
        public void Should_log_GetRecordById_method_called()
        {
            target.GetRecordById("1");
            fakeLogger.Received().Log(Arg.Any<string>());
        }


        [Test]
        public void Should_log_Delete_method_called()
        {
            target.DeleteRecord();
            fakeLogger.Received().Log(Arg.Any<string>());
        }


        [Test]
        public void Should_log_Update_method_called()
        {
            target.UpdateRecord();
            fakeLogger.Received().Log(Arg.Any<string>());
        }


        [Test]
        public void Should_log_AddRecord_method_called()
        {
            target.AddRecord();
            fakeLogger.Received().Log(Arg.Any<string>());
        }

        [Test]
        public void Should_log_DeleteManifest_method_called()
        {
            target.DeleteManifest();
            fakeLogger.Received().Log(Arg.Any<string>());
        }

        [Test]
        public void Should_log_DeleteAgreement_method_called()
        {
            target.DeleteAgreement();
            fakeLogger.Received().Log(Arg.Any<string>());
        }
    }
}