using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ExcelDataReader;
using Marina.UI.Providers.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Marina.UI.Controllers;

[Authorize]
public class ImportController : Controller
{
    private readonly IConfiguration _configuration;
    private static string tblName = "";
    private string DESCId;
    private string Date;
    private IImportRepository _ImportRepository;
    public ImportController(IConfiguration configuration, IImportRepository importRepository)
    {
        _configuration = configuration;
        _ImportRepository = importRepository;


        tblName = SetNameDb();
        DESCId = _ImportRepository.CheckTable(tblName);
        GetPersianDate();
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(IFormFile upload)
    {
        if (ModelState.IsValid)
        {
            if (upload != null && upload.Length > 0)
            {
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                Stream stream = upload.OpenReadStream();
                IExcelDataReader reader;
                if (upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not support");
                    return View();
                }

                DataTable dt = new();
                DataRow row;
                DataTable dt_ = new();

                try
                {
                    dt_ = reader.AsDataSet().Tables[0];
                    dt.Columns.Add("PerDate");

                    for (int i = 1; i < dt_.Columns.Count; i++)
                    {
                        dt.Columns.Add(dt_.Rows[0][i].ToString());
                    }
                    for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                    {
                        row = dt.NewRow();
                        row[0] = Date;
                        for (int col = 1; col < dt_.Columns.Count; col++)
                        {
                            row[col] = dt_.Rows[row_][col].ToString();
                        }
                        dt.Rows.Add(row);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("File", "Unable to upload file!");
                }
                reader.Close();
                reader.Dispose();
                SaveToDataBase(dt);

                return View(dt);
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }

        }
        return View();
    }

    [HttpPost]
    public void Save(DataTable dt)
    {
        // Save
    }

    private async Task<bool> SaveToDataBase(DataTable dataTable)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("MarinaConnectionString");
            if (DESCId is null)
            {
                string query = CreateData(dataTable, tblName);
                using SqlConnection con = new(connectionString);
                SqlCommand command = new(query, con);
                con.Open();
                command.ExecuteNonQuery();
                using (SqlBulkCopy objBulk = new(con))
                {
                    objBulk.DestinationTableName = tblName;

                    foreach (var column in dataTable.Columns)
                    {
                        var sourceColumn = column.ToString();
                        var destinationColumn = column.ToString().Replace(" ", "");

                        objBulk.ColumnMappings.Add(sourceColumn, destinationColumn);
                    }

                    objBulk.WriteToServer(dataTable);

                };
                con.Close();
            }
            else
            {
                //string queryDeleted = $"DELETE FROM {tblName} WHERE PerDate = @DESCId";
                string queryDeleted = $"DELETE FROM {tblName} WHERE PerDate = @DESCId";
                using SqlConnection con = new(connectionString);
                SqlCommand command = new(queryDeleted, con);
                command.Parameters.AddWithValue("@DESCId", DESCId);
                //command.Parameters.AddWithValue("@tblName", tblName);
                con.Open();
                var rowsAffected = command.ExecuteNonQuery();
                con.Close();

                using (SqlBulkCopy bulkCopy = new(con))
                {
                    bulkCopy.DestinationTableName = tblName;
                    //bulkCopy.BatchSize = 100; // تعداد رکورد در هر دسته
                    //bulkCopy.BulkCopyTimeout = 60; // زمان مجاز برای انجام عملیات

                    ColumnMapping(dataTable, bulkCopy);
                    bulkCopy.WriteToServer(dataTable);
                };
            }
        }
        catch (Exception)
        {
            throw;
        }

        return true;
    }

    private static void ColumnMapping(DataTable dataTable, SqlBulkCopy bulkCopy)
    {
        foreach (var column in dataTable.Columns)
        {
            var sourceColumn = column.ToString();
            var destinationColumn = column.ToString();

            bulkCopy.ColumnMappings.Add(sourceColumn, destinationColumn);
        }
    }

    private static string CreateData(DataTable dataTable, string dbName)
    {
        var query = $"USE [MarinaDb2]; CREATE TABLE DBO.[{dbName}] (Id INT IDENTITY(1, 1), ";
        var column = "PerDate nvarchar(4) NOT NULL,";
        foreach (var item in dataTable.Columns)
        {
            column = item.ToString().Replace(" ", "");
            query += $"{column} nvarchar(100) NULL,";
        }
        query = query.Substring(0, query.Length - 1);
        query += ");";
        return query;
    }

    public static System.Data.DataTable? UseSystemTextJson(string sampleJson)
    {
        System.Data.DataTable? dataTable = new();
        if (string.IsNullOrWhiteSpace(sampleJson))
        {
            return dataTable;
        }

        JsonElement data = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(sampleJson);
        if (data.ValueKind != JsonValueKind.Array)
        {
            return dataTable;
        }

        var dataArray = data.EnumerateArray();
        JsonElement firstObject = dataArray.First();

        var firstObjectProperties = firstObject.EnumerateObject();
        dataTable.Columns.Add("Id");
        foreach (var element in firstObjectProperties)
        {
            dataTable.Columns.Add(element.Name);
        }

        int i = 1;
        foreach (var obj in dataArray)
        {
            var objProperties = obj.EnumerateObject();
            DataRow newRow = dataTable.NewRow();
            newRow["Id"] = i;
            foreach (var item in objProperties)
            {
                newRow[item.Name] = item.Value;
            }
            dataTable.Rows.Add(newRow);
            i++;
        }

        return dataTable;
    }
    private string SetNameDb()
    {
        var httpContextAccessor = new HttpContextAccessor();
        var province = httpContextAccessor.HttpContext?.User.FindFirstValue("Province");
        var distributorCode = httpContextAccessor.HttpContext?.User.FindFirstValue("DistributorCode");
        var line = httpContextAccessor.HttpContext?.User.FindFirstValue("Line");
        tblName = $"{distributorCode}_{province}_{line}";
        return tblName;
    }

    private void GetPersianDate()
    {
        System.Globalization.PersianCalendar persianCalandar = new System.Globalization.PersianCalendar();
        var dateTime = DateTime.Now;
        string year = persianCalandar.GetYear(dateTime).ToString().Substring(2, 2);
        string month = persianCalandar.GetMonth(dateTime).ToString("0#");
        //int day = persianCalandar.GetDayOfMonth(dateTime);
        Date = $"{year}{month}";
    }
}
//private static string SetDbName()
//{
//    ClaimsPrincipal currentUser = this.User;
//    ClaimsIdentity currentIdentity = currentUser.Identity as ClaimsIdentity;
//    string firstName = currentIdentity.Name;
//    string lastName = currentIdentity.FindFirst("LastName")?.Value;
//    string str = "";
//    var date = DateTime.Now.ToString("yyyy/MM/dd/HH:mm");
//    var dateStr = date.Replace("/", "").Replace(":", "");
//    //var firstName = user.Claims.FirstOrDefault(c => c.Type == "FirstName");
//    //var lastName = user.Claims.FirstOrDefault(c => c.Type == "LastName");
//    str = $"{dateStr}_{firstName}_{lastName}";
//    return str;
//}

//public string MyAction()
//{
//    var DistributorCode = User.FindFirstValue("DistributorCode");
//    var Province = User.FindFirstValue("Province");
//    var Line = User.FindFirstValue("Line");
//    // ...
//    var str = $"{DistributorCode}_{Province}_{Line}";
//    return str;
//}

//    var roleClaim = HttpContext.User.FindFirst("role");
//if (roleClaim != null)
//{
//    var role = roleClaim.Value;
//    // Do something with the role
//}
//else
//{
//    // Handle the case where the user does not have the "role" claim
//}

//}




//ClaimsPrincipal currentUser = this.User;
//ClaimsIdentity currentIdentity = currentUser.Identity as ClaimsIdentity;
//var a  = currentIdentity.FindFirst("Province")?.Value;

//var Province = HttpContext.User.FindFirstValue("Province");
//var DistributorCode = User.FindFirstValue("DistributorCode");
//var Line = User.FindFirstValue("Line");


//var httpContextAccessor = new HttpContextAccessor();
//var roleClaim = httpContextAccessor.HttpContext?.User.FindFirst("role");
//if (roleClaim != null)
//{
//    var role = roleClaim.Value;
//    // Do something with the role
//}
//else
//{
//    // Handle the case where the user does not have the "role" claim
//}




//public Import()
//{
//    var identity = (ClaimsIdentity)User.Identity;
//    IEnumerable<Claim> claims = identity.Claims;

//    var nameClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name);
//    ClaimsPrincipal currentUser = this.User;
//    ClaimsIdentity currentIdentity = currentUser.Identity as ClaimsIdentity;
//    string firstName = currentIdentity.Name;
//    string lastName = currentIdentity.FindFirst("LastName")?.Value;
//    var date = DateTime.Now.ToString("yyyy/MM/dd/HH:mm");
//    var dateStr = date.Replace("/", "").Replace(":", "");
//    //var firstName = user.Claims.FirstOrDefault(c => c.Type == "FirstName");
//    //var lastName = user.Claims.FirstOrDefault(c => c.Type == "LastName");
//    str = $"{dateStr}_{firstName}_{lastName}";

//}
//public IActionResult MyAction()
//{
//    var jwtToken = "myJwtToken";
//    var token = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
//    var claims = token.Claims;
//    var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
//    var name = nameClaim.Value;

//    // Use the name in your code
//    // ...

//    return View();
//}