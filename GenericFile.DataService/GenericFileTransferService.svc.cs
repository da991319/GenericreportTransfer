using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

namespace GenericFile.DataService
{
    [ServiceContract]
    public class GenericFileTransferService
    {
        [OperationContract]
        public List<Report> GetAllReports()
        {
            return Business.GetAllReports();
        }

        [OperationContract]
        public List<Column> GetColumnsByReport(int reportId)
        {
            return Business.GetColumnsByReport(reportId);
        }

        [OperationContract]
        public void UpsertReport(Report report)
        {
            Business.UpsertReport(report);
        }

        [OperationContract]
        public void DeleteReport(Report report)
        {
            Business.DeleteReport(report);
        }

        [OperationContract]
        public void UpsertColumn(Column column)
        {
            Business.UpsertColumn(column);
        }

        [OperationContract]
        public void DeleteColumn(Column column)
        {
            Business.DeleteColumn(column);
        }

        [OperationContract]
        public List<Transfer> GetTransferMapping(int reportFromId, int reportToId)
        {
            return Business.GetTransferMappping(reportFromId,reportToId);
        }

        [OperationContract]
        public void UpsertTransfer(List<Transfer> transfers)
        {
            Business.UpsertTransfer(transfers);
        }

        [OperationContract]
        public void InsertTempTransfer(List<TempTransfer> tempTransfers)
        {
            Business.InsertTempTransfer(tempTransfers);
        }

        [OperationContract]
        public string GetTempTransferValue(long rowNumber, int colIndex)
        {
            return Business.GetTempTransferValue(rowNumber, colIndex);
        }
    } 
}
