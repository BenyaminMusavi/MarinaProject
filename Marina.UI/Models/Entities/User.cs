using System;
using System.ComponentModel.DataAnnotations;

namespace Marina.UI.Models.Entities;

public partial class User
{
    [Key]
    public int Id { get; set; }
    public string EmailAddress { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
  //  public string Name { get; set; }
   // public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    //public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
  //  public string? Email { get; set; }
 //   public string Password { get; set; } = null!;
    //public bool IsAdmin { get; set; }
    public string UserName { get; set; }
    public string AgencyCode { get; set; }
    public string Line { get; set; }
    public string Province { get; set; }

    #region  Audit Fiels 
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public long? UpdaterUserId { get; set; }
    public DateTime? UpdateTime { get; set; }
    #endregion
}
