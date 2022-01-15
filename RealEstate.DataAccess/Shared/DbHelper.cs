using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Models;
using RealEstate.Data.StoredProc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
namespace RealEstate.DataAccess
{
    public static class DbHelper
    {
        public static List<T> ExecuteQuery<T>(this RealEstateContext db, string query) where T : class, new()
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                db.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    var lst = new List<T>();
                    var lstColumns = new T().GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();
                    while (reader.Read())
                    {
                        var newObject = new T();
                        for (var i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            PropertyInfo prop = lstColumns.FirstOrDefault(a => a.Name.ToLower().Equals(name.ToLower()));
                            if (prop == null)
                            {
                                continue;
                            }
                            var val = reader.IsDBNull(i) ? null : reader[i];
                            prop.SetValue(newObject, val, null);
                        }
                        lst.Add(newObject);
                    }

                    return lst;
                }
            }
        }

        public static DataTable ExecuteQuery(this RealEstateContext db, string query)
        {
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                db.Database.OpenConnection();

                using (var reader = command.ExecuteReader())
                {
                    DataTable result = new DataTable();
                    result.Load(reader);
                    return result;
                }
            }
        }
    }
    public static class SqlProcedures
    { 
        public static List<ExtraContrcat> GetExtraContrcat(RealEstateContext context, int ProjectID,string contractExtraName)
        {
            return DbHelper.ExecuteQuery<ExtraContrcat>(context, $"EXEC dbo.ExtraContrcat @ProjectID = '{ProjectID}' ,@ContractExtraName=N'{contractExtraName}'");
        }
        public static List<CustomerCard> GetCustomerCard(RealEstateContext context, int ContractID)
        {
            return DbHelper.ExecuteQuery<CustomerCard>(context, $"EXEC dbo.CustomerCard @ContractID = '{ContractID}'");
        }  
        public static List<Alert> GetAlert(RealEstateContext context, int ProjectID, DateTime FromDate,DateTime ToDate)
        {
            if(ProjectID==0)
            {
                return DbHelper.ExecuteQuery<Alert>(context, $"EXEC dbo.Alert @ProjectID = null,@FromDate='{FromDate}',@ToDate='{ToDate}'");


            }
            return DbHelper.ExecuteQuery<Alert>(context, $"EXEC dbo.Alert @ProjectID = '{ProjectID}',@FromDate='{FromDate}',@ToDate='{ToDate}'");
        }
        public static List<Alert> GetOverdue(RealEstateContext context, int ProjectID, DateTime FromDate)
        {
            if (ProjectID == 0)
            {
                return DbHelper.ExecuteQuery<Alert>(context, $"EXEC dbo.Overdue @ProjectID = null,@FromDate='{FromDate}'");

            }
            return DbHelper.ExecuteQuery<Alert>(context, $"EXEC dbo.Overdue @ProjectID = '{ProjectID}',@FromDate='{FromDate}'");
        }
        public static List<PrintBill> GetPrintBill(RealEstateContext context, int ContractID) 
        {

            return DbHelper.ExecuteQuery<PrintBill>(context, $"EXEC dbo.PrintBill @ContractID = '{ContractID}'");
        }
    }
}
