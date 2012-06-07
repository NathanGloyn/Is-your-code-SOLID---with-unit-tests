using System;
using System.Data;
using BoxInformation.Interfaces;
using BoxInformation.Model;

namespace BoxInformation.Presenter
{

    public class BoxDetailsPresenter
    {
        private IView view;
        private IDataAccess dataAccess;
        private BoxEntry box;

        public BoxDetailsPresenter(IView view, IDataAccess dataAccess)
        {
            if (view == null) throw new Exception("view cannot be null");

            this.view = view;
            this.dataAccess = dataAccess;
            box = new BoxEntry(view, dataAccess);
        }

        public void GetSearchResults()
        {
            string sql = "SELECT * FROM BoxDetails WHERE ClientName LIKE '%";
            if (!string.IsNullOrEmpty(view.ClientName))
            {
                sql += view.ClientName;
            }
            sql += "%'";
            sql += " AND ClientNumber LIKE '%";
            if (!string.IsNullOrEmpty(view.ClientNumber))
            {
                sql += view.ClientNumber;
            }
            sql += "%'";
            sql += " AND ClientLeader LIKE '%";
            if (!string.IsNullOrEmpty(view.ClientPrincipal))
            {
                sql += view.ClientPrincipal;
            }
            sql += "%'";

            view.searchResults = dataAccess.FillDataSet(sql, CommandType.Text);

        }

        public void GetRecordById(string RecordID)
        {
            box.Get(RecordID);
        }

        public void DeleteRecord()
        {
            box.Delete();
        }

        public void UpdateRecord()
        {
            box.Update();
        }

        public void AddRecord()
        {
            box.Add();
        }

        public void DeleteManifest()
        {
            box.DeleteManifest();
        }

        public void DeleteAgreement()
        {
            box.DeleteAgreement();
        }

    }
}
