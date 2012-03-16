using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GenericFileTransferClient.GenericFileTransferService;
using Microsoft.VisualBasic.FileIO;
using NPOI.HSSF.UserModel;
using OfficeOpenXml;
using System.Linq;
using GenericFileTransferClient.Model;
using System.Text.RegularExpressions;

namespace GenericFileTransferClient
{
    public static class Utils
    {
        private static string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Template");
        private static GenericFileTransferServiceClient serviceClient = new GenericFileTransferServiceClient();

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

                    // Getting the worksheet
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

        public static void ExecuteTransfer(string filePath, Report reportFrom, Report reportTo, List<TransferModel> columnsFrom)
        {
            List<TempTransfer> listCells = new List<TempTransfer>();

            if (reportFrom.FileName.ToLower().Contains(".csv"))
            {
                
            }
            else if (reportFrom.FileName.ToLower().Contains(".xls"))
            {
                //get cells
                listCells = ReadXLS(filePath, reportFrom, columnsFrom);
                WriteXLS(Path.Combine(directoryPath, reportTo.FileName), listCells, reportTo);

                listCells.Clear();
                listCells.TrimExcess();
            }
            else if (reportFrom.FileName.ToLower().Contains(".xlsx"))
            {
                
            }

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

        private static List<TempTransfer> ReadXLS(string filePath, Report reportFrom, List<TransferModel> columnsFrom)
        {
            int row = reportFrom.ResultRow;
            HSSFRow currentRow;
            int colToIndex = -1;
            int currentColId = -1;
            List<TempTransfer> tempList = new List<TempTransfer>();

            //get the col index of columns being mapped 
            List<int> columnsFromIndex = reportFrom.Columns.Where(c => columnsFrom.Where(cm => !cm.Position.Equals(-1))
                .Select(cm => cm.ColumnId).Contains(c.Id)).Select(c => c.Position - 1).ToList();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                //getting complete workbook
                HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);

                // Getting the worksheet
                HSSFSheet sheet = templateWorkbook.GetSheet(reportFrom.SheetName) as HSSFSheet;

                IEnumerator rowEnum = sheet.GetRowEnumerator();

                while (rowEnum.MoveNext())
                {
                    if (rowEnum.Current != null)
                    {
                        currentRow = rowEnum.Current as HSSFRow;
                    
                        //be sure we are dealing with a results row
                        if (currentRow.RowNum >= reportFrom.ResultRow - 1)
                        {
                            foreach (HSSFCell currentCell in currentRow.Cells.Where(c => columnsFromIndex.Contains(c.ColumnIndex)))
                            {
                                //NPOI is 0 based, value are stored in starting from 1
                                currentColId = reportFrom.Columns.Where(c => c.Position.Equals(currentCell.ColumnIndex + 1)).FirstOrDefault().Id;
                                colToIndex = columnsFrom.Where(c => c.ColumnId.Equals(currentColId)).FirstOrDefault().Position;

                                if (colToIndex != -1)
                                {
                                    tempList.Add(new TempTransfer
                                    {
                                        ColIndex = colToIndex ,
                                        RowNumber = currentCell.RowIndex + 1,
                                        Value = currentCell.StringCellValue
                                    });
                                }
                            }
                        }
                    }
                }

                //serviceClient.InsertTempTransfer(tempList);
                tempList.TrimExcess();
                return tempList;
            }
        }

        private static void WriteXLS(string pathTemplate, List<TempTransfer> listTransfers, Report reportTo)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),"Targetfile" + DateTime.Now.ToString("yyy_MM_dd_hh_mm") + ".xls");
            int row = reportTo.ResultRow;
            HSSFRow currentRow;
            HSSFCell currentCell;

            using (FileStream fs = new FileStream(pathTemplate, FileMode.Open, FileAccess.Read))
            {
                //getting complete workbook
                HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);

                // Getting the worksheet
                HSSFSheet sheet = templateWorkbook.GetSheet(reportTo.SheetName) as HSSFSheet;

                foreach (TempTransfer item in listTransfers.OrderBy(t => t.RowNumber).ThenBy(t => t.ColIndex))
                {
                    currentRow = sheet.GetRow(Convert.ToInt32(reportTo.ResultRow - 1 + item.RowNumber - 1)) as HSSFRow;

                    //check if row exists
                    if (currentRow == null)
                    {
                        currentRow = sheet.CreateRow(Convert.ToInt32(reportTo.ResultRow - 1 + item.RowNumber - 1)) as HSSFRow;
                    }

                    currentCell = currentRow.GetCell(item.ColIndex - 1) as HSSFCell;

                    if (currentCell == null)
                    {
                        currentCell = currentRow.CreateCell(item.ColIndex - 1) as HSSFCell;
                    }

                    //hardcoded for the LIMS Template
                    //need to make it more generic
                    if (item.ColIndex.Equals(3) && !Regex.IsMatch(item.Value.ToUpper(), @"\d{2,3}/\d{2}-\d{2}-\d{3}-\d{2}W\d/\d{2}"))
                    {
                        //Not a UWI
                        currentCell.SetCellValue(item.Value);
                    }
                    else
                    {
                        Dictionary<string, string> uwi = UWI.ParseUWIAlberta(item.Value);

                        currentCell.SetCellValue(uwi["wellIdent"]);
                        currentRow.CreateCell(3).SetCellValue(uwi["wellLegalSub"]);
                        currentRow.CreateCell(3).SetCellValue(uwi["wellSection"]);
                        currentRow.CreateCell(3).SetCellValue(uwi["wellTownShip"]);
                        currentRow.CreateCell(3).SetCellValue(uwi["wellRange"]);
                        currentRow.CreateCell(3).SetCellValue(uwi["wellMeridian"]);
                    }
                    
                }

                using (FileStream writer = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    templateWorkbook.Write(writer);
                }

                sheet.Dispose();
                templateWorkbook.Dispose();
            }
        }

    }
}
