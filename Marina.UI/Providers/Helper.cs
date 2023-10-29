﻿using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Marina.UI.Providers;

public static class Helper
{
    public static string GetPersianDate()
    {
        System.Globalization.PersianCalendar persianCalandar = new();
        var dateTime = DateTime.Now;
        string year = persianCalandar.GetYear(dateTime).ToString().Substring(2, 2);
        string month = persianCalandar.GetMonth(dateTime).ToString("0#");
        //int day = persianCalandar.GetDayOfMonth(dateTime);
        return $"{year}{month}";
    }

    public static string SetNameDb()
    {
        var httpContextAccessor = new HttpContextAccessor();
        var province = httpContextAccessor.HttpContext?.User.FindFirstValue("Province");
        var distributorCode = httpContextAccessor.HttpContext?.User.FindFirstValue("DistributorCode");
        var line = httpContextAccessor.HttpContext?.User.FindFirstValue("Line");
        var tblName = $"{distributorCode}_{province}_{line}";
        return tblName;
    }

    public static void ColumnMapping(DataTable dataTable, SqlBulkCopy bulkCopy)
    {
        foreach (var column in dataTable.Columns)
        {
            var sourceColumn = column.ToString();
            var destinationColumn = column.ToString().Replace(" ", "");
            bulkCopy.ColumnMappings.Add(sourceColumn, destinationColumn);
        }
    }

    public static string CreateData(DataTable dataTable, string dbName)
    {
        var query = $"USE [MarinaDb2]; CREATE TABLE DBO.[{dbName}] (Id INT IDENTITY(1, 1), ";
        foreach (var item in dataTable.Columns)
        {
            string? column = item.ToString().Replace(" ", "");
            query += $"{column} nvarchar(100) NULL,";
        }
        query = query.Substring(0, query.Length - 1);
        query += ");";
        return query;
    }
}
