using Marina.UI.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

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
        try
        {
            foreach (var column in dataTable.Columns)
            {
                var sourceColumn = column.ToString();
                var destinationColumn = column.ToString().Replace(" ", "");
                bulkCopy.ColumnMappings.Add(sourceColumn, destinationColumn);
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public static string CreateData(DataTable dataTable,string databaseName, string tblName)
    {
        var query = $"USE [{databaseName}]; CREATE TABLE DBO.[{tblName}] (Id INT IDENTITY(1, 1), ";
        foreach (var item in dataTable.Columns)
        {
            string? column = item.ToString().Replace(" ", "");
            query += $"{column} nvarchar(100) NULL,";
        }
        query = query.Substring(0, query.Length - 1);
        query += ");";
        return query;
    }

    public static void EnsureDatabaseMigrated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MarinaDbContext>();
        var pendingMigrations = dbContext.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
        }
    }
}
