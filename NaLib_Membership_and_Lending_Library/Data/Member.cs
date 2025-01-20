using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Membership_and_Lending_Lib.Data;

[Table("members")]
[Index("Email", Name = "UQ__members__AB6E6164D71FBF5C", IsUnique = true)]
[Index("Email", Name = "idx_members_email")]
public partial class Member
{
    [Key]
    [Column("member_id")]
    public int MemberId { get; set; }

    [Column("name")]
    [StringLength(255)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("phone")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column("postal_address", TypeName = "text")]
    public string? PostalAddress { get; set; }

    [Column("physical_address", TypeName = "text")]
    public string? PhysicalAddress { get; set; }

    [Column("date_enrolled")]
    public DateOnly DateEnrolled { get; set; }

    [Column("status")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Status { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<LendingPreference> LendingPreferences { get; set; } = new List<LendingPreference>();

    [InverseProperty("Member")]
    public virtual ICollection<LendingTransaction> LendingTransactions { get; set; } = new List<LendingTransaction>();
}
