using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Marina.UI.Providers.Repositories;

public interface IImportRepository
{
    string CheckTable(string tableName);
    Task<bool> SaveToDataBase(DataTable dt);
}

public class ImportRepository : IImportRepository
{
    private readonly IConfiguration _configuration;
    private  string DESCId = "";
    private readonly string tblName = "";
    private readonly string connectionString = "";
    private readonly string databaseName = "";
    public ImportRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("MarinaConnectionString");
        tblName = Helper.SetNameDb();
        databaseName = "[MarinaDb2]";
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
        SqlTransaction transaction = con.BeginTransaction();
        DESCId = CheckTable(tblName);
        try
        {
            if (DESCId is null)
            {
                string query = Helper.CreateData(dataTable, tblName);
                SqlCommand command = new(query, con, transaction);
                con.Open();
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
                transaction.Commit();

            }
            else
            {
                var Date = Helper.GetPersianDate();
                string queryDeleted = $"DELETE FROM {tblName} WHERE PerDate = @Date";
                SqlCommand command = new(queryDeleted, con, transaction);
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
                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            transaction.Commit();
            throw;
        }
        return true;
    }
}
