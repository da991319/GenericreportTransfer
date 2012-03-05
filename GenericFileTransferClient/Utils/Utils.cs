using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF;
using NPOI.HSSF.UserModel;
using OfficeOpenXml;

namespace GenericFileTransferClient
{
    public static class Utils
    {
        private static string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Template");

        public static Dictionary<int, string> GetHeaders(string filePath, string separator, int rowHeaders, string sheetName)
        {
            if (Path.GetExtension(filePath).ToLower().Equals(".csv"))
            {
                return GetHeadersCSV(filePath, separator);
            }
            else if (Path.GetExtension(filePath).ToLower().Equals(".xls"))
            {
                return GetHeadersXLS(filePath,sheetName, rowHeaders);
            }
            else if (Path.GetExtension(filePath).ToLower().Equals(".xlsx"))
            {
                return GetHeadersXLSX(filePath, sheetName, rowHeaders);
            }
            else
            {
                return new Dictionary<int, string>();
            }
        }

        private static Dictionary<int, string> GetHeadersCSV(string filePath, string separator)
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

        private static Dictionary<int, string> GetHeadersXLS(string filePath,string sheetName, int rowHeaders)
        {
            Dictionary<int, string> tempList = new Dictionary<int, string>();
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    //getting complete workbook
                    HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);

                    // Getting the 1st worksheet
                    HSSFSheet sheet = templateWorkbook.GetSheet(sheetName) as HSSFSheet;

                    //getting the row header
                    //NPOI is 0 based
                    HSSFRow headerRow = sheet.GetRow(rowHeaders - 1) as HSSFRow;

                    foreach (HSSFCell cell in headerRow.Cells)
                    {
                        tempList.Add(cell.ColumnIndex + 1, cell.StringCellValue);
                    }
                    
                }
            }
            return tempList;
        }

        private static Dictionary<int, string> GetHeadersXLSX(string filePath,string sheetName, int rowHeaders)
        {
            Dictionary<int, string> tempList = new Dictionary<int, string>();
            int column = 1;
            if (File.Exists(filePath))
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    //get the worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];

                    ExcelRange cell = worksheet.Cells[rowHeaders, column];

                    while (!String.IsNullOrWhiteSpace(cell.Text))
                    {
                        tempList.Add(column, cell.Text);
                        column++;
                        cell = worksheet.Cells[rowHeaders, column];
                    }
                }
            }
            return tempList;
        }
        public static void SaveTemplateFile(string filePath)
        {
            //check if template folder exist
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (File.Exists(filePath))
            {
                File.Copy(filePath, Path.Combine(directoryPath, GetFileName(filePath)),true);
            }
        }

        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public static void DeleteTemplate(string fileName)
        {
            if (!String.IsNullOrWhiteSpace(fileName))
            {
                File.Delete(Path.Combine(directoryPath, fileName));
            }
        }
    }
}
