using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

public partial class UserDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("userID")]
    public Guid UserId { get; set; }

    [Column("qualification")]
    [StringLength(50)]
    [Unicode(false)]
    public string Qualification { get; set; } = null!;

    [Column("eperience")]
    public long Eperience { get; set; }

    [Column("skills", TypeName = "text")]
    public string Skills { get; set; } = null!;

    [Column("employment_date")]
    public DateOnly EmploymentDate { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserDetails")]
    public virtual User User { get; set; } = null!;
}
