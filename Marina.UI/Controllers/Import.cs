using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web;
using ExcelDataReader;
using Marina.UI.Models;
using Marina.UI.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Marina.UI.Controllers;

public class Import : Controller
{
       private static string str = "";

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

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ExcelFileReader()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(IFormFile upload)
    {
        if (ModelState.IsValid)
        {
            if (upload != null && upload.Length > 0)
            {
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                Stream stream = upload.OpenReadStream();
                IExcelDataReader reader = null;
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

                DataTable dt = new DataTable();
                DataRow row;
                DataTable dt_ = new DataTable();

                try
                {
                    dt_ = reader.AsDataSet().Tables[0];
                    for (int i = 0; i < dt_.Columns.Count; i++)
                    {
                        dt.Columns.Add(dt_.Rows[0][i].ToString());

                    }
                    for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                    {
                        row = dt.NewRow();
                        for (int col = 0; col < dt_.Columns.Count; col++)
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
                await SaveToDataBase(dt);
                return View(dt);
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }

        }
        return View();
    }
    //public ActionResult Upload(FormCollection formCollection)
    //{
    //    if(Request != null)
    //    {
    //        HttpPostedFileBase
    //    }
    //}

    private static async Task<bool> SaveToDataBase(DataTable dataTable)
    {
        try
        {
            //var dataTable = UseSystemTextJson(str);
            var column = "";
            var query = "use [MarinaDb]; CREATE TABLE DBO.BEN ( ";
            foreach (var item in dataTable.Columns)
            {
                column = item.ToString().Replace(" ", "");
                query += $"{column} nvarchar(100) NULL,";
            }
            query = query.Substring(0, query.Length - 1);
            query += ");";
            string connection = "Data Source=.;Initial Catalog=MarinaDb;integrated security=true;TrustServerCertificate=True";
            SqlConnection con = new SqlConnection(connection);
            SqlBulkCopy objBulk = new SqlBulkCopy(con);
            objBulk.DestinationTableName = str;
            //objBulk.DestinationTableName = "BEN";
            con.Open();
            SqlCommand command = new SqlCommand(query, con);
            command.ExecuteNonQuery();
            objBulk.WriteToServer(dataTable);
            con.Close();
        }
        catch (Exception)
        {
            throw;
        }
        return true;
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

    private string SetDbName()
    {
        ClaimsPrincipal currentUser = this.User;
        ClaimsIdentity currentIdentity = currentUser.Identity as ClaimsIdentity;
        string firstName = currentIdentity.Name;
        string lastName = currentIdentity.FindFirst("LastName")?.Value;
        string str = "";
        var date = DateTime.Now.ToString("yyyy/MM/dd/HH:mm");
        var dateStr = date.Replace("/", "").Replace(":", "");
        //var firstName = user.Claims.FirstOrDefault(c => c.Type == "FirstName");
        //var lastName = user.Claims.FirstOrDefault(c => c.Type == "LastName");
        str = $"{dateStr}_{firstName}_{lastName}";
        return str;

    }

    //public IActionResult MyAction()
    //{
    //    ClaimsPrincipal currentUser = this.User;
    //    ClaimsIdentity currentIdentity = currentUser.Identity as ClaimsIdentity;
    //    string username = currentIdentity.Name;

    //    // Use the username in your code
    //    // ...

    //    return View();
    //}

}
