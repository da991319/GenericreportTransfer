using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace GenericFileTransferClient
{
    public static class Utils
    {
        public static Dictionary<int, string> UploadReport(string filePath, string separator)
        {
            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                Dictionary<int, string> tempList = new Dictionary<int, string>();
                if (separator.Equals(","))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.Delimiters = new string[] { separator };
                }

                string[] headers = parser.ReadFields();

                for (int i = 0; i < headers.Length; i++)
                {
                    tempList.Add(i, headers[i]);
                }

                return tempList;
            }
        }
    }
}
