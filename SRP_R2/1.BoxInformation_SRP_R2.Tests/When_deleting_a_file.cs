using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Model;
using NSubstitute;
using NUnit.Framework;

namespace _1.BoxInformation_SRP_R2.Tests
{
    [TestFixture]
    public class When_deleting_a_file
    {
        private IDataAccess fakeDataAccess;
        private IView fakeView;
        private BoxEntry target;

        [SetUp]
        public void Setup()
        {
            fakeDataAccess = Substitute.For<IDataAccess>();
            fakeView = Substitute.For<IView>();
            target = new BoxEntry(fakeView, fakeDataAccess);
        }


        [Test]
        public void Should_call_data_access_layer()
        {
            target.DeleteManifest();

            fakeDataAccess.ReceivedWithAnyArgs().ExecuteNonQuery(Arg.Any<string>(), Arg.Any<CommandType>(), Arg.Any<IDbDataParameter[]>());
        }
     
        [Test]
        public void Should_pass_correct_stored_proc_name()
        {
            target.DeleteManifest();

            fakeDataAccess.Received().ExecuteNonQuery("DeleteFile1", CommandType.StoredProcedure, Arg.Any<IDbDataParameter[]>());
        }

        [Test]
        public void Should_create_parameter()
        {
            fakeView.Id.Returns("1");

            target.DeleteManifest();

            fakeDataAccess.Received().CreateParameter("@ID", "1");
        }

    }
}