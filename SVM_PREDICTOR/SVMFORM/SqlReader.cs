using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SVMFORM
{
    class SqlReader
    {
        private string currentTable;
        private string connectionString;
        private SqlConnection sqlConnection;
        /*HashSet<int> permit = new HashSet<int>() { 76, 77, 78, 84, 86, 90, 91, 92, 93, 99, 101, 105, 106, 107, 108, 114, 116, 120, 121, 122, 123, 131, 136, 137, 138, 146, 151,
                152, 153, 159, 161, 165, 166, 167, 168, 174, 176, 180, 181, 182, 183, 189, 195, 196, 197, 198, 204, 206, 210, 211, 212, 213, 219, 221, 225, 226, 227, 228, 234, 236,
                240, 241, 242, 243, 249, 251, 255, 256, 257, 258, 264, 266, 270, 271, 272, 273, 279, 281, 285, 286, 287, 288, 300, 301, 302, 303, 315, 316, 317, 318, 330, 331, 332,
                333, 345, 376, 378, 391, 392, 393, 401, 406, 407, 408, 414, 416, 420, 421, 422, 423, 429, 431, 435, 451, 452, 453, 459, 461, 465, 466, 467, 468, 474, 476, 480, 500,
                503, 504, 506, 510, 511, 550 };*/
        HashSet<int> permit = new HashSet<int>() { 76, 77, 78, 90, 91, 93, 105, 106, 107, 108, 195, 197, 210, 225, 243, 270, 300, 452, 468, 500 };
        private int startId = 1023;
        private int tempId = 1023;
        public SqlReader(string connectionString, string dataTableName)
        {
            sqlConnection = new SqlConnection(connectionString);
            this.connectionString = connectionString;
            currentTable = dataTableName;
        }
        public Data GetNextData()
        {
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
