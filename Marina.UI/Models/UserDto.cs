namespace Marina.UI.Models;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string AgencyCode { get; set; }
    public string Line { get; set; }
    public string Province { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
}
