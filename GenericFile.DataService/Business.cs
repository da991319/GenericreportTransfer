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
    }
}