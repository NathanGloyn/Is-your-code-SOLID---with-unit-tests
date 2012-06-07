using System;
using System.Collections.Generic;
using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Model;
using NSubstitute;
using NUnit.Framework;
using Satisfyr;

namespace _4.BoxInformation_OCP.Tests
{
    [TestFixture]
    public class When_retrieving_an_item
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
        public void Should_populate_view()
        {
            var fakeData = ViewData();
            fakeData.Tables[0].Rows.Add("abc", "123","","2012-05-01 00:00:00","","","","true","");

            fakeDataAccess.FillDataSet("",CommandType.StoredProcedure,null).ReturnsForAnyArgs(fakeData);

            target.Get("123");

            fakeView.Satisfies(view => view.ClientName == "abc" 
                                    && view.ClientNumber == "123"
                                    && view.ClientPrincipal == ""
                                    && view.reviewDate == new DateTime(2012,05,01)
                                    && view.comments == ""
                                    && view.fileName == ""
                                    && view.fileName2 == ""
                                    && view.SecureStorage
                                    && view.boxDetails == null);
        }

        [Test]
        public void Should_populate_box_details()
        {
            var fakeData = ViewData();
            fakeData.Tables[0].Rows.Add("abc", "123", "", "2012-05-01 00:00:00", "", "", "", "true", "*Bristol,1*Swindon,2");

            fakeDataAccess.FillDataSet("", CommandType.StoredProcedure, null).ReturnsForAnyArgs(fakeData);

            target.Get("123");

            var expected = new List<KeyValuePair<string, int>>()
                               {
                                   {new KeyValuePair<string, int>("Bristol", 1)}, 
                                   {new KeyValuePair<string, int>("Swindon", 2)}
                               };

            CollectionAssert.AreEquivalent(expected, fakeView.boxDetails);
        }

        private DataSet ViewData()
        {
            var table = new DataTable();

            var columns = new DataColumn[]
                              {
                                  column("ClientName"),
                                  column("ClientNumber"),
                                  column("ClientLeader"),
                                  column("ReviewDate", typeof(DateTime)),
                                  column("Comments"),
                                  column("FileLocation"),
                                  column("FileLocation2"),
                                  column("SecureStorage", typeof(bool)),
                                  column("BoxDetails")
                              };

            table.Columns.AddRange(columns);

            var data = new DataSet();
            data.Tables.Add(table);

            return data;
        }

        private DataColumn column(string columnName, Type columnType = null )
        {
            Type dataType = columnType ?? typeof(string);

            return  new DataColumn(columnName, dataType);
        }
    }
}