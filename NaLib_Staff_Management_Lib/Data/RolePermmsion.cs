using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

public partial class RolePermmsion
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("roleID")]
    public Guid RoleId { get; set; }

    [Column("permissionID")]
    public Guid PermissionId { get; set; }

    [ForeignKey("PermissionId")]
    [InverseProperty("RolePermmsions")]
    public virtual Permission Permission { get; set; } = null!;
}
