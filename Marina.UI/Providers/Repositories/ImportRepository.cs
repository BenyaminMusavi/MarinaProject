using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;

namespace Marina.UI.Providers.Repositories
{
    public interface IImportRepository
    {
        string CheckTable(string tableName);
    }
    public class ImportRepository : IImportRepository
    {
        private readonly IConfiguration _configuration;

        public ImportRepository(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        public string CheckTable(string tableName)
        {
            string connectionString = _configuration.GetConnectionString("MarinaConnectionString");
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                var query = $"SELECT TOP (1) * FROM [MarinaDb2].[dbo].{tableName} ORDER BY Id DESC ";
                SqlCommand command = new(query, connection, transaction);
                SqlDataReader reader = command.ExecuteReader();
                string datTime = "";
                while (reader.Read())
                {
                    // خواندن داده‌ها
                    datTime = reader.GetString(1).ToString();
                }

                reader.Close();
                transaction.Commit();
                return datTime;
            }
            catch (Exception ex) //Microsoft.Data.SqlClient.SqlException  : Microsoft.Data.SqlClient.SqlException
            {
                transaction.Rollback();
                Console.WriteLine("خطا در اجرای کوئری: " + ex.Message);
                return null;
            }

        }












        //private IActionResult LoadAndSaveExcel(IFormFile upload)
        //{
        //    Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //    Stream stream = upload.OpenReadStream();
        //    IExcelDataReader reader;
        //    if (upload.FileName.EndsWith(".xls"))
        //    {
        //        reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //    }
        //    else if (upload.FileName.EndsWith(".xlsx"))
        //    {
        //        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("File", "This file format is not support");
        //        return View();
        //    }

        //    DataTable dt = new();
        //    DataRow row;
        //    DataTable dt_ = new();

        //    try
        //    {
        //        dt_ = reader.AsDataSet().Tables[0];
        //        for (int i = 0; i < dt_.Columns.Count; i++)
        //        {
        //            dt.Columns.Add(dt_.Rows[0][i].ToString());

        //        }
        //        for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
        //        {
        //            row = dt.NewRow();
        //            for (int col = 0; col < dt_.Columns.Count; col++)
        //            {
        //                row[col] = dt_.Rows[row_][col].ToString();
        //            }
        //            dt.Rows.Add(row);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError("File", "Unable to upload file!");
        //    }
        //    reader.Close();
        //    reader.Dispose();
        //    //SaveToDataBase(dt);

        //    return View(dt);
        //}

    }
}
