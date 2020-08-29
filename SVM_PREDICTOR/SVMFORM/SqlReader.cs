using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SVMFORM
{
    class SqlReader
    {
        //Read data from database
        private string currentTable;
        private SqlConnection sqlConnection;
        HashSet<int> permit;
        private int startId;
        private int tempId;
        public SqlReader(string connectionString, string dataTableName)
        {
            //Sets new sql reader
            sqlConnection = new SqlConnection(connectionString);
            currentTable = dataTableName;
            permit = new HashSet<int>() { 76, 77, 78, 90, 91, 93, 105, 106, 107, 108, 195, 197, 210, 225, 243, 270, 300, 452, 468, 500 };
            startId = 1023;
            tempId = startId;
        }
        public Data GetNextData()
        {
            //Read next data vector from datatable
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand("Select * FROM "+currentTable+" WHERE id="+(tempId++).ToString(), sqlConnection).ExecuteReader();
            if (sqlDataReader.Read())
            {
                int shift = 0;
                Data data = new Data(permit.Count);
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    if (!permit.Contains(i))
                    {
                        shift++;
                        continue;
                    }
                    if (i<512)
                        data[i - shift] = sqlDataReader.GetFloat(i);
                    else
                        data[i - shift] = Convert.ToDouble(sqlDataReader.GetBoolean(i));
                }
                sqlDataReader.Close();
                sqlConnection.Close();
                return data;
            }
            else
            {
                sqlDataReader.Close();
                sqlConnection.Close();
                return new Data(permit.Count);
            }
        }
        public DataTable GetLastNData(int N)
        {
            //Read last N data vectors from the end of datatable
            sqlConnection.Open();
            int shift;
            SqlDataReader sqlDataReader = new SqlCommand("Select COUNT(*) from " + currentTable, sqlConnection).ExecuteReader();
            sqlDataReader.Read();
            int rowsCount = sqlDataReader.GetInt32(0);
            sqlDataReader.Close();
            sqlDataReader = new SqlCommand("Select * FROM " + currentTable + " WHERE id>=" + (startId+rowsCount-N).ToString(), sqlConnection).ExecuteReader();
            double[][] dataTable = new double[N][];
            DataTable dTable = new DataTable();
            for (int i = 0; i< N; i++)
            {
                sqlDataReader.Read();
                dataTable[i] = new double[permit.Count];
                shift = 0;
                for (int j = 0; j < sqlDataReader.FieldCount; j++)
                {
                    if (!permit.Contains(j))
                    {
                        shift++;
                        continue;
                    }
                    else
                    if (j < 512)
                    {
                        dataTable[i][j - shift] = sqlDataReader.GetFloat(j);
                    }
                    else
                        dataTable[i][j - shift] = Convert.ToDouble(sqlDataReader.GetBoolean(j));
                }
            }
            for (int j = 0; j < permit.Count; j++)
                dTable.Columns.Add("Column"+j.ToString(), typeof(double));
            for(int i = 0; i<N; i++)
            {
                var row = dTable.NewRow();
                for (int j = 0; j < permit.Count; j++)
                {
                    row[j] = dataTable[i][j];
                }
                dTable.Rows.Add(row);
            }
            sqlDataReader.Close();
            sqlConnection.Close();
            return dTable;
        }
    }
}
