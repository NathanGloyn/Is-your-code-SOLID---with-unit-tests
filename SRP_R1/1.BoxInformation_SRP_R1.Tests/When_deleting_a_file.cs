using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Presenter;
using NSubstitute;
using NUnit.Framework;

namespace _1.BoxInformation_SRP_R1.Tests
{
    [TestFixture]
    public class When_deleting_a_file
    {
        private IDataAccess fakeDataAccess;
        private IView fakeView;
        private BoxDetailsPresenter target;

        [SetUp]
        public void Setup()
        {
            fakeDataAccess = Substitute.For<IDataAccess>();
            fakeView = Substitute.For<IView>();
            target = new BoxDetailsPresenter(fakeView, fakeDataAccess);
        }


        [Test]
        public void Should_call_data_access_layer()
        {
            target.DeleteFile1();

            fakeDataAccess.ReceivedWithAnyArgs().ExecuteNonQuery(Arg.Any<string>(), Arg.Any<CommandType>(), Arg.Any<IDbDataParameter[]>());
        }
     
        [Test]
        public void Should_pass_correct_stored_proc_name()
        {
            target.DeleteFile1();

            fakeDataAccess.Received().ExecuteNonQuery("DeleteFile1", CommandType.StoredProcedure, Arg.Any<IDbDataParameter[]>());
        }

        [Test]
        public void Should_create_parameter()
        {
            fakeView.Id.Returns("1");

            target.DeleteFile1();

            fakeDataAccess.Received().CreateParameter("@ID", "1");
        }

    }
}