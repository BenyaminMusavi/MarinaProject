using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Marina.UI.Providers.Repositories;

public interface IImportRepository
{
    Task<bool> SaveToDatabase(DataTable dt);
    Task<DataTable> GetAll();
}

public class ImportRepository : IImportRepository
{
    private readonly string tableName;
    private readonly string? connectionString;
    private readonly string? databaseName;

    public ImportRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("MarinaConnectionString");
        tableName = Helper.SetTableName();
        databaseName = configuration.GetValue<string>("Database:Name");
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
            var tableExists = await Helper.TableExists(tableName);
            using (SqlBulkCopy bulkCopy = new(con))
            {
                try
                {
                    con.Open();

                    if (!tableExists)
                    {
                        var query = CreateQueryTable(dataTable, databaseName, tableName);
                        using (SqlCommand command = new(query, con))
                        {
                            command.ExecuteNonQuery();
                        }
                    }

                    var destinationColumnName = SelectColumnNameTable(tableName);
                    var isValidColumnMapping = ColumnMapping(dataTable, bulkCopy, destinationColumnName);
                    if (!isValidColumnMapping)
                        return false;

                    var Date = Helper.GetPersianDate();
                    var queryDeleted = $"DELETE FROM {tableName} WHERE PerDate = @Date";
                    using (SqlCommand commandDeleted = new(queryDeleted, con))
                    {
                        commandDeleted.Parameters.AddWithValue("@Date", Date);
                        var rowsAffected = commandDeleted.ExecuteNonQuery();
                    }
                    bulkCopy.DestinationTableName = tableName;
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
    public async Task<DataTable> GetAll()
    {
        var tableExists = await Helper.TableExists(tableName);
        DataTable dataTable = new();
        if (tableExists)
        {
            using SqlConnection connection = new(connectionString);
            {
                connection.Open();
                var query = $"SELECT * FROM {databaseName}.[dbo].{tableName}";
                SqlDataAdapter adapter = new(query, connection);
                adapter.Fill(dataTable);
            }
        }
        return dataTable;
    }

    private static bool ColumnMapping(DataTable dataTable, SqlBulkCopy bulkCopy, List<string> destinationColumnNames)
    {
        try
        {
            //foreach (var column in dataTable.Columns)
            //{
            //    var sourceColumn = column.ToString();
            //    var destinationColumn = column.ToString().Replace(" ", "");
            //    bulkCopy.ColumnMappings.Add(sourceColumn, destinationColumn);
            //}
            foreach (DataColumn column in dataTable.Columns)
            {
                string sourceColumn = column.ColumnName;

                var isValid = destinationColumnNames.Contains(sourceColumn);
                if (isValid)
                    bulkCopy.ColumnMappings.Add(sourceColumn, sourceColumn);
                else
                    return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error mapping columns: " + ex.Message);
        }
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

    private static string CreateQueryTable(DataTable dataTable, string databaseName, string tableName)
    {
        var queryBuilder = new StringBuilder();

        queryBuilder.Append($"USE [{databaseName}]; CREATE TABLE DBO.[{tableName}] (Id INT IDENTITY(1, 1), ");

        foreach (DataColumn column in dataTable.Columns)
        {
            queryBuilder.Append($"[{column.ColumnName}] nvarchar(MAX) NULL,");
        }

        queryBuilder.Remove(queryBuilder.Length - 1, 1);
        queryBuilder.Append(");");

        return queryBuilder.ToString();
    }
    //static string ValidateColumnName(string columnName)
    //{        // الگوی اعتبارسنجی: حروف انگلیسی و حروف فارسی و کاراکتر underscore
    //     string pattern = "[a-zA-Zآ-ی_]+";
    //    // استفاده از عبارت منظم برای اعتبارسنجی و تبدیل کاراکترهای اضافی به underscore
    //    string validatedColumnName = Regex.Replace(column.ColumnName, $"[^{pattern}]", "_");
    //    return validatedColumnName;
    //}
    //public static string CreateQueryTable(DataTable dataTable, string databaseName, string tblName)
    //{
    //    var query = $"USE [{databaseName}]; CREATE TABLE DBO.[{tblName}] (Id INT IDENTITY(1, 1), ";
    //    foreach (var item in dataTable.Columns)
    //    {
    //        string? column = item.ToString().Replace(" ", "");
    //        query += $"{column} nvarchar(100) NULL,";
    //    }
    //    query = query.Substring(0, query.Length - 1);
    //    query += ");";
    //    return query;
    //}

    public async Task<bool> SaveToDatabase2(DataTable dataTable)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            var tableExists = await Helper.TableExists(tableName);

            using (SqlBulkCopy bulkCopy = new(connection))
            {
                try
                {
                    connection.Open();

                    if (!tableExists)
                    {
                        CreateTable(connection, tableName, dataTable);
                    }

                    var destinationColumnName = SelectColumnNameTable(tableName);
                    var isValidColumnMapping = ColumnMapping(dataTable, bulkCopy, destinationColumnName);

                    if (!isValidColumnMapping)
                    {
                        return false;
                    }

                    var date = Helper.GetPersianDate();
                    var queryDeleted = $"DELETE FROM {tableName} WHERE PerDate = @Date";

                    using (SqlCommand commandDeleted = new(queryDeleted, connection))
                    {
                        commandDeleted.Parameters.AddWithValue("@Date", date);
                        var rowsAffected = commandDeleted.ExecuteNonQuery();
                    }

                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving data: " + ex.Message);
                    return false;
                }
            }
        }
        return true;
    }

    private void CreateTable(SqlConnection connection, string tableName, DataTable dataTable)
    {
        var query = $"CREATE TABLE {tableName} ({string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => $"{c.ColumnName} {GetSqlType(c.DataType)}"))})";
        using (SqlCommand command = new(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }
    private string GetSqlType(Type type)
    {
        if (type == typeof(int))
        {
            return "INT";
        }
        else if (type == typeof(string))
        {
            return "NVARCHAR(MAX)";
        }
        else if (type == typeof(DateTime))
        {
            return "DATETIME";
        }
        else
        {
            throw new ArgumentException($"Unsupported data type: {type}");
        }
    }
}
