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
        private void GetReader()
        {
            //Creates new Reader associated with directory
            stream = File.Open(currentFile, FileMode.Open, FileAccess.Read);
            reader = ExcelReaderFactory.CreateReader(stream);
        }
        public DataReader(string filesDirectory)
        {
            //Create new DataReader in directory
            currentDirecory = filesDirectory;
            UnreadUpdate();
            GetReader();
            GetNextData();
        }
        public void UnreadUpdate()
        {
            //Updates unread files in current directory
            listOfUnreadFiles = new List<string>(Directory.GetFiles(currentDirecory));
            foreach (string file in listOfUnreadFiles)
                if (listOfReadFiles.IndexOf(file) != -1)
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
            //Gets next Data vector
            if (reader.Read())
            { 
                Data data = new Data(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                    data[i] = reader.GetDouble(i);
                return data;
            }
            else
            {
                GetReader();
                return GetNextData();
            }
        }
        public double[][] GetDataTable(string sheet = "sheet1")
        {
            //Gets whole data table
            return reader.AsDataSet().Tables[sheet].ToJagged();
           
        }
    }
}
