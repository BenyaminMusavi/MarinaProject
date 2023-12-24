using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;

namespace Marina.UI.Providers.Repositories;

public interface IImportRepository
{
    Task<bool> TableExists(string tableName);
    Task<bool> SaveToDatabase(DataTable dt);
    Task<DataTable> GetAll();
}

public class ImportRepository : IImportRepository
{
    private readonly string tblName;
    private readonly string? connectionString;
    private readonly string? databaseName;

    public ImportRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("MarinaConnectionString");
        tblName = Helper.SetTableName();
        databaseName = configuration.GetValue<string>("Database:Name");
    }

    public async Task<bool> TableExists(string tableName)
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();

        var query = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
        SqlCommand command = new(query, connection);
        var result = await command.ExecuteScalarAsync();

        return result != null;
    }

    //public async Task<bool> SaveToDatabase(DataTable dataTable)
    //{
    //    using SqlConnection con = new(connectionString);
    //    var tableExists = await TableExists(tblName);
    //    using SqlBulkCopy bulkCopy = new(con);
    //    try
    //    {
    //        bool isValidColumnMapping = true;
    //        con.Open();

    //        if (tableExists)
    //        {
    //            var destinationColumnName = SelectColumnNameTable(tblName);
    //            bulkCopy.DestinationTableName = tblName;
    //            isValidColumnMapping = Helper.ColumnMapping(dataTable, bulkCopy, destinationColumnName);
    //        }

    //        if (!tableExists)
    //        {
    //            var query = Helper.CreateData(dataTable, databaseName, tblName);
    //            SqlCommand command = new(query, con);
    //            command.ExecuteNonQuery();
    //        }

    //        var Date = Helper.GetPersianDate();
    //        var queryDeleted = $"DELETE FROM {tblName} WHERE PerDate = @Date";
    //        SqlCommand commandDeleted = new(queryDeleted, con);
    //        commandDeleted.Parameters.AddWithValue("@Date", Date);
    //        var rowsAffected = commandDeleted.ExecuteNonQuery();

    //        if (isValidColumnMapping)
    //            await bulkCopy.WriteToServerAsync(dataTable);
    //        else
    //            return false;

    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("خطا در ذخیره سازی داده ها: " + ex.Message);
    //        return false;
    //    }
    //    return true;
    //}

    public async Task<bool> SaveToDatabase(DataTable dataTable)
    {
        using (SqlConnection con = new(connectionString))
        {
            var tableExists = await TableExists(tblName);
            using (SqlBulkCopy bulkCopy = new(con))
            {
                try
                {
                    bool isValidColumnMapping = true;
                    con.Open();

                    if (tableExists)
                    {
                        var destinationColumnName = SelectColumnNameTable(tblName);
                        bulkCopy.DestinationTableName = tblName;
                        isValidColumnMapping = Helper.ColumnMapping(dataTable, bulkCopy, destinationColumnName);
                    }
                    if (!isValidColumnMapping)
                        return false;

                    if (!tableExists)
                    {
                        var query = Helper.CreateData(dataTable, databaseName, tblName);
                        using (SqlCommand command = new(query, con))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    var Date = Helper.GetPersianDate();
                    var queryDeleted = $"DELETE FROM {tblName} WHERE PerDate = @Date";
                    using (SqlCommand commandDeleted = new(queryDeleted, con))
                    {
                        commandDeleted.Parameters.AddWithValue("@Date", Date);
                        var rowsAffected = commandDeleted.ExecuteNonQuery();
                    }

                    await bulkCopy.WriteToServerAsync(dataTable);


                }
                catch (Exception ex)
                {
                    Console.WriteLine("خطا در ذخیره سازی داده ها: " + ex.Message);
                    return false;
                }
            }
        }
        return true;
    }


    private List<string> SelectColumnNameTable(string tblName)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = new SqlCommand($"SELECT * FROM {tblName}", connection);
        var reader = command.ExecuteReader();
        var columnNames = Enumerable.Range(0, reader.FieldCount)
            .Select(i => reader.GetName(i))
            .ToList();
        return columnNames;
    }

    public async Task<DataTable> GetAll()
    {
        var tableExists = await TableExists(tblName);
        DataTable dataTable = new();
        if (tableExists)
        {
            using SqlConnection connection = new(connectionString);
            {
                connection.Open();
                var query = $"SELECT * FROM {databaseName}.[dbo].{tblName}";
                SqlDataAdapter adapter = new(query, connection);
                adapter.Fill(dataTable);
            }
        }
        return dataTable;
    }
}
