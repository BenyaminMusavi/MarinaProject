using System;

namespace Marina.UI.Models;

public class CookieUserItem
{
    public int UserId { get; set; }
    public string EmailAddress { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}