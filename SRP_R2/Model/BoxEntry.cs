using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using BoxInformation.Interfaces;

namespace BoxInformation.Model
{
    public class BoxEntry
    {
        private IDataAccess dataAccess;
        private IView view;

        public BoxEntry(IView view, IDataAccess dataAccess)
        {
            if (view == null) throw new ArgumentNullException("view");

            this.view = view;
            this.dataAccess = dataAccess;
        }

        public void Get(string boxId)
        {
            DataSet entry = dataAccess.FillDataSet("GetRecordByID", CommandType.StoredProcedure, dataAccess.CreateParameter("@ID", boxId));

            if (entry.Tables.Count < 1 || entry.Tables.Count > 1)
            {
                throw new InvalidOperationException("Multiple data tables returned when only one expected");
            }

            if (entry.Tables[0].Rows.Count != 1)
            {
                throw new InvalidOperationException("Multiple rows of data returned when only one expected");
            }

            PopulateView(entry.Tables[0].Rows[0]);

        }

        public void Delete()
        {
            dataAccess.ExecuteNonQuery("DeleteRecord", CommandType.StoredProcedure, dataAccess.CreateParameter("@ID", view.Id));
        }

        public void Update()
        {
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();

            parameters.Add(dataAccess.CreateParameter("@ID", view.Id));
            parameters.Add(dataAccess.CreateParameter("@ClientName", view.ClientName));
            parameters.Add(dataAccess.CreateParameter("@ClientNumber", view.ClientNumber));
            parameters.Add(dataAccess.CreateParameter("@ClientLeader", view.ClientPrincipal));
            parameters.Add(dataAccess.CreateParameter("@ReviewDate", view.reviewDate.GetValueOrDefault(new DateTime(1900, 01, 01))));
            parameters.Add(dataAccess.CreateParameter("@Comments", view.comments));
            parameters.Add(dataAccess.CreateParameter("@FileLocation", view.fileName));
            parameters.Add(dataAccess.CreateParameter("@FileLocation2", view.fileName2));
            parameters.Add(dataAccess.CreateParameter("@SecureStorage", view.SecureStorage));
            parameters.Add(dataAccess.CreateParameter("@BoxDetails", ConvertKVPToString(view.boxDetails)));

            if (view.file.ContentLength > 0)
            {
                view.file.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(view.file.FileName));
                parameters[6].Value = Path.GetFileName(view.file.FileName);
            }

            if (view.file2.ContentLength > 0)
            {
                view.file2.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(view.file2.FileName));
                parameters[7].Value = Path.GetFileName(view.file2.FileName);
            }

            dataAccess.ExecuteNonQuery("UpdateRecord", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void Add()
        {
            List<IDbDataParameter> parameters = new List<IDbDataParameter>();

            parameters.Add(dataAccess.CreateParameter("@ClientName", view.ClientName));
            parameters.Add(dataAccess.CreateParameter("@ClientNumber", view.ClientNumber));
            parameters.Add(dataAccess.CreateParameter("@ClientLeader", view.ClientPrincipal));
            parameters.Add(dataAccess.CreateParameter("@ReviewDate", view.reviewDate.GetValueOrDefault(new DateTime(1900, 01, 01))));
            parameters.Add(dataAccess.CreateParameter("@Comments", view.comments));
            parameters.Add(dataAccess.CreateParameter("@FileLocation", view.fileName));
            parameters.Add(dataAccess.CreateParameter("@FileLocation2", view.fileName2));
            parameters.Add(dataAccess.CreateParameter("@SecureStorage", view.SecureStorage));
            parameters.Add(dataAccess.CreateParameter("@BoxDetails", ConvertKVPToString(view.boxDetails)));

            if (view.file.ContentLength > 0)
            {
                view.file.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(view.file.FileName));
                parameters[5].Value = Path.GetFileName(view.file.FileName);
            }

            if (view.file2.ContentLength > 0)
            {
                view.file2.SaveAs(ConfigurationManager.AppSettings["FileSaveLocation"] + Path.GetFileName(view.file2.FileName));
                parameters[6].Value = Path.GetFileName(view.file2.FileName);
            }

            dataAccess.ExecuteNonQuery("AddRecord", CommandType.StoredProcedure, parameters.ToArray());

        }

        public void DeleteManifest()
        {
            dataAccess.ExecuteNonQuery("DeleteFile1",CommandType.StoredProcedure, dataAccess.CreateParameter("@ID", view.Id));
        }

        public void DeleteAgreement()
        {
            dataAccess.ExecuteNonQuery("DeleteFile2", CommandType.StoredProcedure, dataAccess.CreateParameter("@ID", view.Id));
        }

        public void PopulateView(DataRow entry)
        {
            if (entry["ClientName"].ToString() != "")
            {
                view.ClientName = entry["ClientName"].ToString();
            }

            if (entry["ClientNumber"].ToString() != "")
            {
                view.ClientNumber = entry["ClientNumber"].ToString();
            }

            if (entry["ClientLeader"].ToString() != "")
            {
                view.ClientPrincipal = entry["ClientLeader"].ToString();
            }

            if (entry["ReviewDate"].ToString() != "")
            {
                view.reviewDate = (DateTime?)entry["ReviewDate"];
            }

            if (entry["Comments"].ToString() != "")
            {
                view.comments = entry["Comments"].ToString();
            }

            if (entry["FileLocation"].ToString() != "")
            {
                view.fileName = entry["FileLocation"].ToString();
            }

            if (entry["FileLocation2"].ToString() != "")
            {
                view.fileName2 = entry["FileLocation2"].ToString();
            }

            if (entry["SecureStorage"].ToString() != "")
            {
                view.SecureStorage = (bool)entry["SecureStorage"];
            }

            if (entry["BoxDetails"].ToString() != "")
            {
                view.boxDetails = ConvertStringToKVP(entry["BoxDetails"].ToString());
            }
        }

        private List<KeyValuePair<string, int>> ConvertStringToKVP(string strEntryString)
        {
            List<KeyValuePair<string, int>> lstResults = new List<KeyValuePair<string, int>>();

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
    }
}