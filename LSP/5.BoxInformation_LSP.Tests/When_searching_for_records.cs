using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Presenter;
using NSubstitute;
using NUnit.Framework;

namespace _5.BoxInformation_LSP.Tests
{
    [TestFixture]
    public class When_searching_for_records
    {
        private IDataAccess fakeDataAccess;
        private ISearchView fakeView;
        private SearchPresenter target;

        [SetUp]
        public void Setup()
        {
            fakeDataAccess = Substitute.For<IDataAccess>();
            fakeView = Substitute.For<ISearchView>();
            target = new SearchPresenter(fakeDataAccess);
            target.SearchView = fakeView;
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
