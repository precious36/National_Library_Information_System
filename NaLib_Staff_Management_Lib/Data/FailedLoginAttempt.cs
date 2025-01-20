using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

[Table("Failed_Login_Attempts")]
public partial class FailedLoginAttempt
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("userID")]
    public Guid UserId { get; set; }

    [Column("attempt_time", TypeName = "datetime")]
    public DateTime AttemptTime { get; set; }

    [Column("ip_adress")]
    [StringLength(64)]
    [Unicode(false)]
    public string IpAdress { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("FailedLoginAttempts")]
    public virtual User User { get; set; } = null!;
}
