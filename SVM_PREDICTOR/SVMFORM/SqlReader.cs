using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Access;
using Accord.Math;
using Accord;
using System.Data;

namespace SVMFORM
{
    class SqlReader
    {
        private SqlConnection sqlConnection;
        private List<string> listOfUnreadTables=new List<string>(), listOfReadTables=new List<string>();
        public string currentTable;
        private SqlDataReader sqlDataReader;
        private int skips = 9;
        private int rowNumber=1;
        public SqlReader(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
            UnreadUpdate();
        }
        public void UnreadUpdate()
        {
            sqlConnection.Open();
            foreach (DataRow row in sqlConnection.GetSchema("Tables").Rows)
                if (!listOfReadTables.Contains(row[2].ToString()))
                    listOfUnreadTables.Add(row[2].ToString());
            sqlConnection.Close();
            SetCurrentTable();
        }
        public void SetCurrentTable()
        { 
            currentTable = listOfUnreadTables[0];
            listOfReadTables.Add(currentTable);
            listOfUnreadTables.Remove(currentTable);
        }
        public void ChangeDataBase(string dataBase)
        {
            sqlConnection.Open();
            sqlConnection.ChangeDatabase(dataBase);
            sqlConnection.Close();
            UnreadUpdate();
        }
        public Data GetNextData()
        {
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand("Select * FROM "+currentTable+" WHERE id="+(rowNumber++).ToString(), sqlConnection).ExecuteReader();
            if (sqlDataReader.Read())
            {
                int shift = 0;
                HashSet<int> zeros = new HashSet<int>() { 57, 116, 117, 118, 119, 124, 125, 126, 127 };
                Data data = new Data(sqlDataReader.FieldCount - skips);
                for (int i = 1; i < sqlDataReader.FieldCount; i++)
                {
                    if (zeros.Contains(i))
                    {
                        shift++;
                        continue;
                    }
                    data[i - shift] = sqlDataReader.GetDouble(i);
                }
                sqlConnection.Close();
                sqlConnection.Close();
                return data;
            }
            else
            {
                sqlDataReader.Close();
                sqlConnection.Close();
                rowNumber = 1;
                return GetNextData();
            }
        }
        public double[][] GetDataTable()
        {
            sqlConnection.Open();
            int shift = 0;
            HashSet<int> zeros = new HashSet<int>() { 57, 116, 117, 118, 119, 124, 125, 126, 127 };
            sqlDataReader = new SqlCommand("Select COUNT(*) from " + currentTable, sqlConnection).ExecuteReader();
            sqlDataReader.Read();
            int rowsCount = sqlDataReader.GetInt32(0);
            sqlDataReader.Close();
            sqlDataReader = new SqlCommand("Select * FROM " + currentTable, sqlConnection).ExecuteReader();
            double[][] dataTable = new double[rowsCount][];
            for (int i = 0; i< rowsCount; i++)
            {
                sqlDataReader.Read();
                dataTable[i] = new double[sqlDataReader.FieldCount-1-skips];
                for (int j = 1; j < sqlDataReader.FieldCount; j++)
                {
                    if(zeros.Contains(j-1))
                    {
                        shift++;
                        continue;
                    }
                    else
                    dataTable[i][j - 1 - shift] = sqlDataReader.GetDouble(j);
                }
            }
            sqlConnection.Close();
            return dataTable;
        }
    }
}
