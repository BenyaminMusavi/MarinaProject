using System.Web;

using Microsoft.AspNetCore.Mvc;

namespace Marina.UI.Controllers;

public class Import : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ExcelFileReader()
    {
        return View();
    }

    //public ActionResult Upload(FormCollection formCollection)
    //{
    //    if(Request != null)
    //    {
    //        HttpPostedFileBase
    //    }
    //}


}
