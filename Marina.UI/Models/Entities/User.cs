using System.ComponentModel.DataAnnotations;

namespace Marina.UI.Models.Entities;

public partial class User
{
    [Key]
    public int Id { get; set; }
    public string DName { get; set; } = null!;
    public int RegionId { get; set; }
    public int RSMId { get; set; }
    public string UserName { get; set; } = null!;
    public int DistributorId { get; set; }
    public int LineId { get; set; }
    public int ProvinceId { get; set; }
    public int NsmId { get; set; }
    public int SupervisorId { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;

    #region  Audit Fiels 
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public bool HasImported { get; set; } = false;
    public DateTime CreateDate { get; set; }
    public long? UpdaterUserId { get; set; }
    public DateTime? UpdateTime { get; set; }
    #endregion

    public virtual Distributor Distributor { get; set; }
    public virtual Line Line { get; set; }
    public virtual Province Province { get; set; }
    public virtual IList<Supervisor> Supervisor { get; set; }
    public virtual Region Region { get; set; }
    public virtual RSM RSM { get; set; }
    public virtual NSM NSM { get; set; }
}

public partial class Region
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public partial class RSM
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public partial class Distributor
{
    [Key]
    public int Id { get; set; }
    public string Code { get; set; } = null!;
}

public partial class Line
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public partial class Province
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public partial class Supervisor
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public virtual IList<User> Users { get; set; }

}

public partial class NotImportedData
{
    public NotImportedData(int supervisorId, string personName)
    {
        SupervisorId = supervisorId;
        PersonName = personName;
    }

    [Key]
    public int Id { get; set; }
    public int SupervisorId { get; set; }
    public string PersonName { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public virtual Supervisor Supervisor { get; set; }

}

public partial class NSM
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}