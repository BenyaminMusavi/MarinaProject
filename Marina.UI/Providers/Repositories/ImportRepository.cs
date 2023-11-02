using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Marina.UI.Providers.Repositories;

public interface IImportRepository
{
    string CheckTable(string tableName);
    Task<bool> SaveToDataBase(DataTable dt);
    DataTable GetAll();
}

public class ImportRepository : IImportRepository
{
    private readonly IConfiguration _configuration;
    private string DESCId = "";
    private readonly string tblName = "";
    private readonly string connectionString = "";
    private readonly string databaseName = "";
    public ImportRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("MarinaConnectionString");
        tblName = Helper.SetNameDb();
        databaseName = _configuration.GetValue<string>("Database:Name");
    }

    public string CheckTable(string tableName)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        //SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            var query = $"SELECT TOP (1) * FROM {databaseName}.[dbo].{tableName} ORDER BY Id DESC ";
            SqlCommand command = new(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            string datTime = reader.GetString(1).ToString();
            reader.Close();
            //transaction.Commit();
            return datTime;
        }
        catch (Exception ex)
        {
            //transaction.Rollback();
            Console.WriteLine("خطا در اجرای کوئری: " + ex.Message);
            return null;
        }
    }

    public async Task<bool> SaveToDataBase(DataTable dataTable)
    {
        using SqlConnection con = new(connectionString);
        DESCId = CheckTable(tblName);
        try
        {
            con.Open();
            //SqlTransaction transaction = con.BeginTransaction();
            if (DESCId is null)
            {
                string query = Helper.CreateData(dataTable, databaseName, tblName);
                SqlCommand command = new(query, con); //transaction
                command.ExecuteNonQuery();
                using (SqlBulkCopy bulkCopy = new(con))
                {
                    foreach (var column in dataTable.Columns)
                    {
                        bulkCopy.DestinationTableName = tblName;
                        Helper.ColumnMapping(dataTable, bulkCopy);
                        bulkCopy.WriteToServer(dataTable);
                    }
                };
                con.Close();
                //transaction.Commit();

            }
            else
            {
                var Date = Helper.GetPersianDate();
                string queryDeleted = $"DELETE FROM {tblName} WHERE PerDate = @Date";
                SqlCommand command = new(queryDeleted, con);
                command.Parameters.AddWithValue("@Date", Date);
                con.Open();
                var rowsAffected = command.ExecuteNonQuery();
                using (SqlBulkCopy bulkCopy = new(con))
                {
                    bulkCopy.DestinationTableName = tblName;
                    Helper.ColumnMapping(dataTable, bulkCopy);
                    bulkCopy.WriteToServer(dataTable);
                };
                con.Close();
                //transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            //transaction.Commit();
            throw;
        }
        return true;
    }

    public DataTable GetAll()
    {
        using SqlConnection connection = new(connectionString);
        DataTable dataTable = new();

        var query = $"SELECT * FROM {databaseName}.[dbo].{tblName} ";
        SqlDataAdapter adapter = new(query, connection);

        connection.Open();
        adapter.Fill(dataTable);
        return dataTable;
    }

    //public async Task<DataTable> GetAllAsync()
    //{
    //    using SqlConnection connection = new(connectionString);
    //    DataTable dataTable = new();

    //    var query = $"SELECT * FROM {databaseName}.[dbo].{tblName} ";
    //    SqlDataAdapter adapter = new(query, connection);

    //    await connection.OpenAsync();
    //    await adapter.FillAsync(dataTable);
    //    return dataTable;
    //}
}
