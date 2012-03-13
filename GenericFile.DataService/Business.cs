using System;
using System.Collections.Generic;

namespace GenericFile.DataService
{
    public static class Business
    {
        public static List<Report> GetAllReports()
        {
            return CRUD.GetAllReports();
        }

        public static List<Column> GetColumnsByReport(int reportId)
        {
            return CRUD.GetColumnsByReport(reportId);
        }

        public static void UpsertReport(Report report)
        {
            CRUD.UpsertReport(report);
        }

        public static void DeleteReport(Report report)
        {
            CRUD.DeleteReport(report);
        }

        public static void UpsertColumn(Column column)
        {
            CRUD.UpsertColumn(column);
        }

        public static void DeleteColumn(Column column)
        {
            CRUD.DeleteColumn(column);
        }

        public static List<Transfer> GetTransferMappping(int reportFromId, int reportToId)
        {
            return CRUD.GetTransferMapping(reportFromId, reportToId);
        }

        public static void UpsertTransfer(List<Transfer> transfers)
        {
            CRUD.UpsertTransfer(transfers);
        }

        public static void InsertTempTransfer(TempTransfer tempTransfer)
        {
            CRUD.InsertTrempTransfer(tempTransfer);
        }

        public static string GetTempTransferValue(long rowNumber, int colIndex)
        {
            return CRUD.GetTempTransferValue(rowNumber, colIndex);
        }
    }
}