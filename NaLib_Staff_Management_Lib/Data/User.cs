using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

public partial class User
{
    [Column("ID")]
    public int Id { get; set; }

    [Key]
    [Column("userID")]
    public Guid UserId { get; set; }

    [Column("first_name")]
    [StringLength(200)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(200)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("email")]
    [StringLength(200)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("departmentID")]
    public Guid DepartmentId { get; set; }

    [Column("roleID")]
    public Guid RoleId { get; set; }

    [Column("password")]
    [StringLength(100)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("initial_password_expired")]
    public bool InitialPasswordExpired { get; set; }

    [Column("password_last_updated", TypeName = "datetime")]
    public DateTime PasswordLastUpdated { get; set; }

    [Column("is_account_locked")]
    public bool IsAccountLocked { get; set; }

    [Column("failed_attempts")]
    public int? FailedAttempts { get; set; }

    [Column("last_failed_attempt", TypeName = "datetime")]
    public DateTime? LastFailedAttempt { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }

    [Column("two_factor_enabled")]
    public bool TwoFactorEnabled { get; set; }

    [Column("libraryID")]
    public Guid LibraryId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<FailedLoginAttempt> FailedLoginAttempts { get; set; } = new List<FailedLoginAttempt>();

    [InverseProperty("User")]
    public virtual ICollection<PasswordHistory> PasswordHistories { get; set; } = new List<PasswordHistory>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<UserDetail> UserDetails { get; set; } = new List<UserDetail>();
}
