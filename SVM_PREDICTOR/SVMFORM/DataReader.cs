using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using Accord.Math;
using ExcelDataReader;

namespace SVMFORM
{
    class DataReader
    {
        //Reading data
        private List<string> listOfUnreadFiles, listOfReadFiles=new List<string>();
        private string currentDirecory;
        private string currentFile;
        private FileStream stream;
        private IExcelDataReader reader;
        private int skips = 9;
        private void GetReader()
        {
            //Creates new Reader associated with directory
            stream = File.Open(currentFile, FileMode.Open, FileAccess.Read);
            reader = ExcelReaderFactory.CreateReader(stream) ;
        }
        public DataReader(string filesDirectory)
        {
            //Create new ExcelDataReader in directory
            currentDirecory = filesDirectory;
            UnreadUpdate();
            GetReader();
        }
        public void UnreadUpdate()
        {
            //Updates unread files in current directory
            listOfUnreadFiles = new List<string>(Directory.GetFiles(currentDirecory));
            foreach (string file in listOfUnreadFiles)
                if (listOfReadFiles.Contains(file))
                    listOfUnreadFiles.Remove(file);
            SetCurrentFile();
        }
        public void ChangeDirectory(string filesDirectory)
        {
            //Change current directory
            currentDirecory = filesDirectory;
            listOfUnreadFiles = new List<string>(Directory.GetFiles(filesDirectory));
            listOfReadFiles = new List<string>();
            UnreadUpdate();
            GetReader();
        }
        public void SetCurrentFile()
        {
            //Sets cureent file
            currentFile = listOfUnreadFiles[0];
            listOfReadFiles.Add(currentFile);
            listOfUnreadFiles.Remove(currentFile);
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
                stream.Close();
                reader.Close();
                GetReader();
                return GetNextData();
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
