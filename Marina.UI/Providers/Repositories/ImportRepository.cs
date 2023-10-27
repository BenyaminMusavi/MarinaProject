using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Marina.UI.Providers.Repositories
{
    public interface IImportRepository
    {
        void CheckTable(string tableName);
    }
    public class ImportRepository : IImportRepository
    {
        private readonly IConfiguration _configuration;

        public ImportRepository(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        public void CheckTable(string tableName)
        {
            string connectionString = _configuration.GetConnectionString("MarinaConnectionString");
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                var query = $"SELECT TOP (1) * FROM [MarinaDb2].[dbo].{tableName} ORDER BY ID DESC ";
                SqlCommand command = new(query, connection,transaction);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // خواندن داده‌ها
                }

                reader.Close();
                transaction.Commit();
            }
            catch (Exception ex) //Microsoft.Data.SqlClient.SqlException  : Microsoft.Data.SqlClient.SqlException
            {
                transaction.Rollback();
                Console.WriteLine("خطا در اجرای کوئری: " + ex.Message);
            }

        }

    }
}
