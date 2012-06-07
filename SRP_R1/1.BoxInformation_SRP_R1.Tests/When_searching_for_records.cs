using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Presenter;
using NSubstitute;
using NUnit.Framework;

namespace _1.BoxInformation_SRP_R1.Tests
{
    [TestFixture]
    public class When_searching_for_records
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
        public void Should_generate_expected_Sql()
        {
            target.GetSearchResults();

            fakeDataAccess.Received().FillDataSet(Arg.Is("SELECT * FROM BoxDetails WHERE ClientName LIKE '%%' AND ClientNumber LIKE '%%' AND ClientLeader LIKE '%%'"),Arg.Any<CommandType>(),Arg.Any<IDbDataParameter[]>());
        }

        [Test]
        public void Should_set_View_Search_Result_property()
        {
            fakeDataAccess.FillDataSet("", CommandType.Text, null).ReturnsForAnyArgs(new DataSet("test"));

            target.GetSearchResults();
            Assert.AreEqual("test", fakeView.searchResults.DataSetName);
        }
    }
}
