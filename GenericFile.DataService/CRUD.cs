using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace GenericFile.DataService
{
    public static class CRUD
    {
        public static List<Report> GetAllReports()
        {
            using (DbEntities db = new DbEntities())
            {
                var results = db.Reports.ToList();
                results.ForEach(r => db.Detach(r));
                return results;
            }
        }

        public static List<Column> GetColumnsByReport(int reportId)
        {
            using (DbEntities db = new DbEntities())
            {
                var results = db.Columns.Where(c => c.ReportId.Equals(reportId)).ToList();
                results.ForEach(c => db.Detach(c));
                return results;
            }
        }

        public static int UpsertReport(Report report)
        {
            using (DbEntities db = new DbEntities())
            {
                if (report.EntityKey == null)
                {
                    //new report so insert
                    db.Reports.AddObject(report);
                    //db.ObjectStateManager.ChangeObjectState(newReport, System.Data.EntityState.Added);
                }
                else
                {
                    //update report
                    db.Attach(report);
                    db.ObjectStateManager.ChangeObjectState(report, System.Data.EntityState.Modified);
                }
                return db.SaveChanges();
            }
        }

        public static int DeleteReport(Report report)
        {
            using (DbEntities db = new DbEntities())
            {
                db.Attach(report);
                db.Reports.DeleteObject(report);
                db.ObjectStateManager.ChangeObjectState(report, System.Data.EntityState.Deleted);
                return db.SaveChanges();
            }
        }

        public static int UpsertColumn(Column column)
        {
            using (DbEntities db = new DbEntities())
            {
                if (column.EntityKey == null)
                {
                    //new column so insert
                    db.Columns.AddObject(column);
                    //db.ObjectStateManager.ChangeObjectState(newReport, System.Data.EntityState.Added);
                }
                else
                {
                    //update report
                    db.Attach(column);
                    db.ObjectStateManager.ChangeObjectState(column, System.Data.EntityState.Modified);
                }
                return db.SaveChanges();
            }
        }

        public static int DeleteColumn(Column column)
        {
            using (DbEntities db = new DbEntities())
            {
                db.Columns.DeleteObject(column);
                //db.ObjectStateManager.ChangeObjectState(report, System.Data.EntityState.Deleted);
                return db.SaveChanges();
            }
        }

        public static List<Transfer> GetTransferMapping(int reportFromId, int reportToId)
        {
            using (DbEntities db = new DbEntities())
            {
                var results = db.Transfers.Where(t => t.ReportFromId.Equals(reportFromId) && t.ReportToId.Equals(reportToId)).ToList();
                results.ForEach(t => db.Detach(t));
                return results;
            }
        }

        public static int UpsertTransfer(List<Transfer> transfers)
        {
            using (DbEntities db = new DbEntities())
            {
                foreach (Transfer transfer in transfers)
                {
                    if (transfer.EntityKey == null)
	                {
                        db.Transfers.AddObject(transfer);
	                }
                    else
                    {
                        db.Attach(transfer);
                        db.ObjectStateManager.ChangeObjectState(transfer, System.Data.EntityState.Modified);
                    }
                }

                return db.SaveChanges();
            }
        }

        public static int InsertTrempTransfer(List<TempTransfer> tempTransfers)
        {
            using (DbEntities db = new DbEntities())
            {
                tempTransfers.ForEach(t => db.AddObject("TempTransfer",t));
                return db.SaveChanges();
            }
        }

        public static string GetTempTransferValue(long rowNumber, int colIndex)
        {
            using (DbEntities db = new DbEntities())
            {
                return db.TempTransfers.Where(t => t.ColIndex.Equals(colIndex) && t.RowNumber.Equals(rowNumber))
                    .FirstOrDefault().Value;
            }
        }
    }
}