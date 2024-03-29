﻿using System.Data;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader;
using Marina.UI.Models.Entities;
using Marina.UI.Providers;
using Marina.UI.Providers.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marina.UI.Controllers;

[Authorize]
public class ImportController : Controller
{
    private readonly IImportRepository _ImportRepository;
    private readonly IUserRepository _userRepository;

    public ImportController(IImportRepository importRepository, IUserRepository userRepository)
    {
        _ImportRepository = importRepository;
        _userRepository = userRepository;
    }

    public IActionResult Index()
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
                if (upload.FileName.EndsWith(".xls") || upload.FileName.EndsWith(".xlsx"))
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Stream stream = upload.OpenReadStream();
                    IExcelDataReader reader;
                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    DataTable dt = CreateDataTable(reader);

                    if (await _ImportRepository.SaveToDatabase(dt))
                    {
                        await _userRepository.HasImported(GetUserId());

                        return View(dt);
                    }
                    ModelState.AddModelError("File", "This file format is not supported");

                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                }
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
        dt.Columns.Add("UserId");
        dt.Columns.Add("PerDate");
        AddColumnsFromExcel(dt, reader);
        AddRowsFromExcel(dt, reader);
        return dt;
    }

    private void AddColumnsFromExcel(DataTable dt, IExcelDataReader reader)
    {
        DataTable dt_ = reader.AsDataSet().Tables[0];
        for (int i = 0; i < dt_.Columns.Count; i++)
        {
            var c = dt_.Rows[0][i].ToString();
            var updateColumn = Regex.Replace(c, @"[^a-zA-Z\u0600-\u06FF]+", "_");

            //dt.Columns.Add(dt_.Rows[0][i].ToString());
            dt.Columns.Add(updateColumn);
        }

        //var column = dt_.Rows[row_][col].ToString();
        //var updateColumn = Regex.Replace(column, @"[^a-zA-Z\u0600-\u06FF]+", "_");
        ////row[col + 2] = dt_.Rows[row_][col].ToString();
        //row[col + 2] = updateColumn;

    }

    private void AddRowsFromExcel(DataTable dt, IExcelDataReader reader)
    {
        using (reader)
        {
            DataTable dt_ = reader.AsDataSet().Tables[0];
            var Date = Helper.GetPersianDate();
            string? userId = GetUserId().ToString();
            for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
            {
                DataRow row = dt.NewRow();
                row["UserId"] = userId;
                row["PerDate"] = Date;

                for (int col = 0; col < dt_.Columns.Count; col++)
                {
                    row[col + 2] = dt_.Rows[row_][col].ToString();
                }
                dt.Rows.Add(row);
            }
        }
    }
    //private DataTable CreateDataTable(IExcelDataReader reader)
    //{
    //    DataTable dt = new();
    //    DataRow row;
    //    DataTable dt_ = new();
    //    var Date = Helper.GetPersianDate();
    //    string? userId = GetUserId().ToString();

    //    try
    //    {
    //        dt_ = reader.AsDataSet().Tables[0];
    //        dt.Columns.Add("UserId");
    //        dt.Columns.Add("PerDate");
    //        for (int i = 0; i < dt_.Columns.Count; i++)
    //        {
    //            dt.Columns.Add(dt_.Rows[0][i].ToString());
    //        }
    //        for (int row_ = 0; row_ < dt_.Rows.Count; row_++)
    //        {
    //            row = dt.NewRow();
    //            row["UserId"] = userId;
    //            row["PerDate"] = Date;

    //            for (int col = 0; col < dt_.Columns.Count; col++)
    //            {
    //                row[col + 2] = dt_.Rows[row_][col].ToString();
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
    //    return dt;
    //}

    private int GetUserId()
    {
        return Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
