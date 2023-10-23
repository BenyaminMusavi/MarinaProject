using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Marina.UI.Models.ViewModels;

public class RegisterVm //: LoginVm
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string AgencyCode { get; set; }
    public LineType Line { get; set; }
    public string Province { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
