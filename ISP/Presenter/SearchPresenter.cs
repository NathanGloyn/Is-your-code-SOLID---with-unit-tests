using System;
using System.Data;
using BoxInformation.Interfaces;

namespace BoxInformation.Presenter
{
    public class SearchPresenter
    {
        private  ISearchView view;
        private readonly IDataAccess dataAccess;

        public ISearchView SearchView
        {
            get { return view; }
            set { view = value; }
        }

        public SearchPresenter(ISearchView view, IDataAccess dataAccess)
        {
            if (view == null) throw new Exception("view cannot be null");
            this.view = view;
            this.dataAccess = dataAccess;
        }

        public void GetSearchResults()
        {
            const string sqlFormat = "SELECT * FROM BoxDetails WHERE ClientName LIKE '%{0}%' AND ClientNumber LIKE '%{1}%' AND ClientLeader LIKE '%{2}%'";

            string sql = string.Format(sqlFormat,
                                       GetFieldValue(view.ClientName),
                                       GetFieldValue(view.ClientNumber),
                                       GetFieldValue(view.ClientPrincipal));

            view.searchResults = dataAccess.FillDataSet(sql, CommandType.Text);

        }

        private static string GetFieldValue(string field)
        {
            string value = null;

            if (!string.IsNullOrEmpty(field))
            {
                value = field;
            }

            return value;
        }
    }
}
