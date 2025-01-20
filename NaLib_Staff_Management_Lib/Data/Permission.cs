using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

public partial class Permission
{
    [Column("ID")]
    public int Id { get; set; }

    [Key]
    [Column("permissionID")]
    public Guid PermissionId { get; set; }

    [Column("resource")]
    [StringLength(200)]
    [Unicode(false)]
    public string Resource { get; set; } = null!;

    [Column("action")]
    [StringLength(20)]
    [Unicode(false)]
    public string Action { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateOnly UpdatedAt { get; set; }

    [InverseProperty("Permission")]
    public virtual ICollection<RolePermmsion> RolePermmsions { get; set; } = new List<RolePermmsion>();
}
