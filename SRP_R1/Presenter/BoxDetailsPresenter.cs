using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Text;
using BoxInformation.Interfaces;

namespace BoxInformation.Presenter
{

    public class BoxDetailsPresenter
    {
        private IView _view;
        private readonly IDataAccess _dataAccess;

        public BoxDetailsPresenter(IView view, IDataAccess dataAccess)
        {
            if (view == null) throw new Exception("view cannot be null");
            _view = view;
            _dataAccess = dataAccess;
        }

        public bool GetSearchResults()
        {
            string sql = "SELECT * FROM BoxDetails WHERE ClientName LIKE '%";
            if (!string.IsNullOrEmpty(_view.ClientName))
            {
                sql += _view.ClientName;
            }   
            sql += "%'";
            sql += " AND ClientNumber LIKE '%";
            if (!string.IsNullOrEmpty(_view.ClientNumber))
            {
                sql += _view.ClientNumber;
            }
            sql += "%'";
            sql += " AND ClientLeader LIKE '%";
            if (!string.IsNullOrEmpty(_view.ClientPrincipal))
            {
                sql += _view.ClientPrincipal;
            }
            sql += "%'";


            _view.searchResults = _dataAccess.FillDataSet(sql, CommandType.Text);

            return true;
        }

        public bool GetEntry(string RecordID)
        {

            DataSet entry = _dataAccess.FillDataSet("GetRecordByID", CommandType.StoredProcedure, _dataAccess.CreateParameter("@ID", RecordID));

            if (entry.Tables[0].Rows[0]["ClientName"].ToString() != "")
            {
                _view.ClientName = entry.Tables[0].Rows[0]["ClientName"].ToString();
            }

            if (entry.Tables[0].Rows[0]["ClientNumber"].ToString() != "")
            {
                _view.ClientNumber = entry.Tables[0].Rows[0]["ClientNumber"].ToString(); 
            }

            if (entry.Tables[0].Rows[0]["ClientLeader"].ToString() != "")
            {
                _view.ClientPrincipal = entry.Tables[0].Rows[0]["ClientLeader"].ToString();
            }

            if (entry.Tables[0].Rows[0]["ReviewDate"].ToString() != "")
            {
                _view.reviewDate = (DateTime?)entry.Tables[0].Rows[0]["ReviewDate"];
            }

            if (entry.Tables[0].Rows[0]["Comments"].ToString() != "")
            {
                _view.comments = entry.Tables[0].Rows[0]["Comments"].ToString();
            }

            if (entry.Tables[0].Rows[0]["FileLocation"].ToString() != "")
            {
                _view.fileName = entry.Tables[0].Rows[0]["FileLocation"].ToString();
            }

            if (entry.Tables[0].Rows[0]["FileLocation2"].ToString() != "")
            {
                _view.fileName2 = entry.Tables[0].Rows[0]["FileLocation2"].ToString();
            }

            if (entry.Tables[0].Rows[0]["SecureStorage"].ToString() != "")
            {
                _view.SecureStorage = (bool)entry.Tables[0].Rows[0]["SecureStorage"];
            }

            if (entry.Tables[0].Rows[0]["BoxDetails"].ToString() != "")
            {
                _view.boxDetails = ConvertStringToKVP(entry.Tables[0].Rows[0]["BoxDetails"].ToString());
            }

            return true;

        }

        public bool DeleteRecord()
        {
            try
            {
               _dataAccess.ExecuteNonQuery("DeleteRecord", CommandType.StoredProcedure, _dataAccess.CreateParameter("@ID", _view.Id));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateRecord()
        {
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();

            parameters.Add(_dataAccess.CreateParameter("@ID", _view.Id));
            parameters.Add(_dataAccess.CreateParameter("@ClientName", _view.ClientName));
            parameters.Add(_dataAccess.CreateParameter("@ClientNumber", _view.ClientNumber));
            parameters.Add(_dataAccess.CreateParameter("@ClientLeader", _view.ClientPrincipal));
            parameters.Add(_dataAccess.CreateParameter("@ReviewDate", _view.reviewDate.GetValueOrDefault(new DateTime(1900, 01, 01))));
            parameters.Add(_dataAccess.CreateParameter("@Comments", _view.comments));

            if (_view.file.ContentLength > 0)
            {
                _view.file.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(_view.file.FileName));
                parameters.Add(_dataAccess.CreateParameter("@FileLocation", Path.GetFileName(_view.file.FileName)));
            }
            else
            {
                parameters.Add(_dataAccess.CreateParameter("@FileLocation", _view.fileName));
            }

            if (_view.file2.ContentLength > 0)
            {
                _view.file2.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(_view.file2.FileName));
                parameters.Add(_dataAccess.CreateParameter("@FileLocation2", Path.GetFileName(_view.file2.FileName)));
            }
            else
            {
                parameters.Add(_dataAccess.CreateParameter("@FileLocation2", _view.fileName2));
            }


            parameters.Add(_dataAccess.CreateParameter("@SecureStorage", _view.SecureStorage));
            parameters.Add(_dataAccess.CreateParameter("@BoxDetails", ConvertKVPToString(_view.boxDetails)));

            

            try
            {
                return _dataAccess.ExecuteNonQuery("UpdateRecord",CommandType.StoredProcedure,parameters.ToArray()) != 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddRecord(string strClientName)
        {
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();

            parameters.Add(_dataAccess.CreateParameter("@ClientName", _view.ClientName));
            parameters.Add(_dataAccess.CreateParameter("@ClientNumber", _view.ClientNumber));
            parameters.Add(_dataAccess.CreateParameter("@ClientLeader", _view.ClientPrincipal));
            parameters.Add(_dataAccess.CreateParameter("@ReviewDate", _view.reviewDate.GetValueOrDefault(new DateTime(1900,01,01))));
            parameters.Add(_dataAccess.CreateParameter("@Comments", _view.comments));
            
            if (_view.file.ContentLength > 0 )
            {
                _view.file.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(_view.file.FileName));
                parameters.Add(_dataAccess.CreateParameter("@FileLocation", Path.GetFileName(_view.file.FileName)));
            }
            else
            {
                parameters.Add(_dataAccess.CreateParameter("@FileLocation", ""));
            }

            if (_view.file2.ContentLength > 0)
            {
                _view.file2.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(_view.file2.FileName));
                parameters.Add(_dataAccess.CreateParameter("@FileLocation2", Path.GetFileName(_view.file2.FileName)));
            }
            else
            {
                parameters.Add(_dataAccess.CreateParameter("@FileLocation2", ""));
            }


            parameters.Add(_dataAccess.CreateParameter("@SecureStorage", _view.SecureStorage));
            parameters.Add(_dataAccess.CreateParameter("@BoxDetails", ConvertKVPToString(_view.boxDetails)));

            try
            {
                return _dataAccess.ExecuteNonQuery("AddRecord",CommandType.StoredProcedure,parameters.ToArray()) != 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        private string UploadFile(HttpPostedFile fleFile)
        {
            byte[] bufFileBuffer = new byte[fleFile.ContentLength];

            fleFile.InputStream.Read(bufFileBuffer, 0, fleFile.ContentLength);

            FileStream newFile = new FileStream(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(fleFile.FileName), FileMode.Create);

            newFile.Write(bufFileBuffer, 0, bufFileBuffer.Length);

            newFile.Close();

            return Path.GetFileName(fleFile.FileName);
        }

        private string ConvertKVPToString(List<KeyValuePair<string, int>> keyValueList)
        {
            if (keyValueList != null)
            {
                StringBuilder resultString = new StringBuilder();
                foreach (KeyValuePair<string, int> keyValue in keyValueList)
                {
                    resultString.Append("*");
                    resultString.Append(keyValue.Key);
                    resultString.Append(",");
                    resultString.Append(keyValue.Value);
                }

                return resultString.ToString();

            }
            else
            {
                return "";
            }
        }

        private List<KeyValuePair<string, int>> ConvertStringToKVP(string strEntryString)
        {
            List<KeyValuePair<string, int>> lstResults = new List<KeyValuePair<string,int>>();

            string[] strResults;
            int result;
            string[] strEntryStringArray = strEntryString.Split('*');

            foreach (string strValue in strEntryStringArray)
            {
                if (strValue.Length > 0)
                {
                    string key = "";
                    int value = 0;
                    strResults = strValue.Split(',');
                    foreach (string strCurrentPair in strResults)
                    {
                        if (int.TryParse(strCurrentPair, out result))
                        {
                            value = int.Parse(strCurrentPair);
                        }
                        else
                        {
                            key = strCurrentPair;
                        }
                    }
                    KeyValuePair<string, int> kvpResults = new KeyValuePair<string, int>(key, value);
                    lstResults.Add(kvpResults);
                }
            }

            return lstResults;
        }

        public void DeleteFile1()
        {
            try
            {
                _dataAccess.ExecuteNonQuery("DeleteFile1",CommandType.StoredProcedure, _dataAccess.CreateParameter("@ID", _view.Id));
            }
            catch (Exception e)
            {
            }
        }

        public void DeleteFile2()
        {
            try
            {
                _dataAccess.ExecuteNonQuery("DeleteFile2", CommandType.StoredProcedure, _dataAccess.CreateParameter("@ID", _view.Id));
            }
            catch (Exception e)
            {
            }
        }
    }
}
