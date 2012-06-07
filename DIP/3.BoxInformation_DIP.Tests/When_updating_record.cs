using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Model;
using NSubstitute;
using NUnit.Framework;

namespace _3.BoxInformation_DIP.Tests
{
    [TestFixture]
    public class When_updating_record
    {
        private IRecordView fakeView;
        private IDataAccess fakeDataAccess;
        private BoxEntry target;

        [SetUp]
        public void Setup()
        {
            fakeView = Substitute.For<IRecordView>();
            fakeDataAccess = Substitute.For<IDataAccess>();
            target = new BoxEntry(fakeDataAccess);
            target.View = fakeView;         
        }

        [Test]
        public void Should_call_data_access()
        {
            target.Update();

            fakeDataAccess.ReceivedWithAnyArgs().ExecuteNonQuery("",CommandType.StoredProcedure, null);
        }

        [Test]
        public void Should_call_data_access_with_correct_stored_procedure_name()
        {
            target.Update();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Is("UpdateRecord"), Arg.Any<CommandType>(), Arg.Any<IDbDataParameter[]>());
        }

        [Test]
        public void Should_call_data_access_with_correct_command_type()
        {
            target.Update();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Any<string>(), Arg.Is(CommandType.StoredProcedure), Arg.Any<IDbDataParameter[]>());            
        }

        [Test]
        public void Should_call_data_access_with_correct_numer_of_parameters()
        {
            target.Update();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Any<string>(), Arg.Any<CommandType>(), Arg.Is<IDbDataParameter[]>(p => p.Length == 10));                        
        }

    }
}