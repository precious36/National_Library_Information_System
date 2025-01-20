using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

[Table("PasswordHistory")]
public partial class PasswordHistory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("userID")]
    public Guid UserId { get; set; }

    [Column("password")]
    [StringLength(100)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PasswordHistories")]
    public virtual User User { get; set; } = null!;
}
