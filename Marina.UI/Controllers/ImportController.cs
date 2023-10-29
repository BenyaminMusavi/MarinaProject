using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ExcelDataReader;
using Marina.UI.Providers;
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
    private IImportRepository _ImportRepository;
    public ImportController(IConfiguration configuration, IImportRepository importRepository)
    {
        _configuration = configuration;
        _ImportRepository = importRepository;
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
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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

                DataTable dt = CreateDataTable(reader);

                _ImportRepository.SaveToDataBase(dt);

                return View(dt);
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }

        }
        return View();
    }

    private DataTable CreateDataTable(IExcelDataReader reader)
    {
        DataTable dt = new();
        DataRow row;
        DataTable dt_ = new();
        var Date = Helper.GetPersianDate();

        try
        {
            dt_ = reader.AsDataSet().Tables[0];
            dt.Columns.Add("PerDate");
            for (int i = 0; i < dt_.Columns.Count; i++)
            {
                dt.Columns.Add(dt_.Rows[0][i].ToString());
            }
            for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
            {
                row = dt.NewRow();
                row["PerDate"] = Date;

                for (int col = 0; col < dt_.Columns.Count; col++)
                {
                    row[col + 1] = dt_.Rows[row_][col].ToString();
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
        return dt;
    }
 }
