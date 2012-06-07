using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Presenter;
using NSubstitute;
using NUnit.Framework;

namespace _1.BoxInformation_SRP_R1.Tests
{
    [TestFixture]
    public class When_updating_record
    {
        private IView fakeView;
        private IDataAccess fakeDataAccess;
        private BoxDetailsPresenter target;

        [SetUp]
        public void Setup()
        {
            fakeView = Substitute.For<IView>();
            fakeDataAccess = Substitute.For<IDataAccess>();
            target = new BoxDetailsPresenter(fakeView, fakeDataAccess);            
        }

        [Test]
        public void Should_call_data_access()
        {
            target.UpdateRecord();

            fakeDataAccess.ReceivedWithAnyArgs().ExecuteNonQuery("",CommandType.StoredProcedure, null);
        }

        [Test]
        public void Should_call_data_access_with_correct_stored_procedure_name()
        {
            target.UpdateRecord();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Is("UpdateRecord"), Arg.Any<CommandType>(), Arg.Any<IDbDataParameter[]>());
        }

        [Test]
        public void Should_call_data_access_with_correct_command_type()
        {
            target.UpdateRecord();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Any<IDbDataParameter[]>());            
        }

        [Test]
        public void Should_call_data_access_with_correct_numer_of_parameters()
        {
            target.UpdateRecord();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Any<string>(), Arg.Any<CommandType>(), Arg.Is<IDbDataParameter[]>(p => p.Length == 10));                        
        }

    }
}