using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Model;
using NSubstitute;
using NUnit.Framework;

namespace _1.BoxInformation_SRP_R2.Tests
{
    [TestFixture]
    public class When_adding_records
    {
        private IView fakeView;
        private IDataAccess fakeDataAccess;
        private BoxEntry target;

        [SetUp]
        public void Setup()
        {
            fakeView = Substitute.For<IView>();
            fakeDataAccess = Substitute.For<IDataAccess>();
            target = new BoxEntry(fakeView, fakeDataAccess);
        }

        [Test]
        public void Should_call_data_access()
        {
            
            target.Add();
            fakeDataAccess.ReceivedWithAnyArgs().ExecuteNonQuery("", CommandType.StoredProcedure,null);            
        }

        [Test]
        public void Should_call_data_access_correctly()
        {
            target.Add();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Is("AddRecord"), Arg.Is(CommandType.StoredProcedure),
                                                       Arg.Any<IDbDataParameter[]>());
        }

        [Test] 
        public void Should_pass_9_parameters()
        {
            target.Add();

            fakeDataAccess.Received().ExecuteNonQuery(Arg.Any<string>(), Arg.Any<CommandType>(),
                                                      Arg.Is<IDbDataParameter[]>(d => d.Length == 9));
        }



    }
}