using BoxInformation.Interfaces;
using BoxInformation.Presenter;
using NSubstitute;
using NUnit.Framework;

namespace _2.BoxInformation_ISP.Tests
{
    [TestFixture]
    public class When_calling_RecordPresenter
    {
        private IBoxEntry fakeBox;
        private RecordPresenter target;

        [SetUp]
        public void Setup()
        {
            fakeBox = Substitute.For<IBoxEntry>();
            target = new RecordPresenter(fakeBox);
        }

        [Test]
        public void Should_call_Get_on_box_entry()
        {
            target.GetRecordById("1");
            fakeBox.Received().Get("1");
        }

        [Test] 
        public void Should_call_Delete_on_box_entry()
        {
            target.DeleteRecord();
            fakeBox.Received().Delete();
        }

        [Test]
        public void Should_call_Update_on_box_entry()
        {
            target.UpdateRecord();
            fakeBox.Update();
        }

        [Test]
        public void Should_call_Add_on_box_entry()
        {
            target.AddRecord();
            fakeBox.Add();
        }

        [Test]
        public void Should_call_DeleteManifest_on_box_entry()
        {
            target.DeleteManifest();
            fakeBox.Received().DeleteManifest();
        }

        [Test]
        public void Should_call_DeleteAgreement_on_box_entry()
        {
            target.DeleteAgreement();
            fakeBox.Received().DeleteAgreement();
        }
    }
}