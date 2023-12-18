namespace Marina.UI.Models.ViewModels;

public class RegisterVm //: LoginVm
{
    public string UserName { get; set; }
    public int LineId { get; set; }
    public int ProvinceId { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string DistributorName { get; set; } = null!;
    public int RegionId { get; set; }
    public int RSMId { get; set; }
    public int DistributorId { get; set; }
    public string PhoneNumber { get; set; }
    public int SupervisorId { get; set; }

}