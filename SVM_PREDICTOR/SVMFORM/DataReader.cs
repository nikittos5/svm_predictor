using System.Collections.Generic;
using System.IO;
using System.Data;
using ExcelDataReader;

namespace SVMFORM
{
    class DataReader
    {
        //Reading data
        private FileStream stream;
        private IExcelDataReader reader;
        private int skips = 9;
        private void GetReader(string filePath)
        {
            //Creates new Reader associated with directory
            stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            reader = ExcelReaderFactory.CreateReader(stream);
        }
        public DataReader(string filePath)
        {
            //Create new ExcelDataReader in directory
            GetReader(filePath);
        }
        public Data GetNextData()
        {
            //Gets next Data vector and skips zeros
            if (reader.Read())
            {
                int shift = 0;
                HashSet<int> zeros = new HashSet<int>() { 57, 116, 117, 118, 119, 124, 125, 126, 127 };
                Data data = new Data(reader.FieldCount - skips);
                for (int i = 0; i < reader.FieldCount; i++)
                {     
                    if (zeros.Contains(i))
                    {
                        shift++;
                        continue;
                    }
                    data[i-shift] = reader.GetDouble(i);
                }
                return data;
            }
            else
            {
                return new Data();
            }
        }
        public DataTable GetDataTable(string sheet = "sheet1")
        {
            //Gets whole data table
            HashSet<int> zeros = new HashSet<int>() { 57, 116, 117, 118, 119, 124, 125, 126, 127 };
            DataTable dataTable = reader.AsDataSet().Tables[sheet];
            int columnsCount = dataTable.Columns.Count-1;
            for(int i = columnsCount; i>=0; i--)
            {
                if (zeros.Contains(i)) dataTable.Columns.Remove(dataTable.Columns[i]);
            }
            return dataTable;
           
        }
    }
}
