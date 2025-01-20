using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Membership_and_Lending_Lib.Data;

[Table("lending_preferences")]
[Index("MemberId", Name = "idx_lending_preferences_member_id")]
public partial class LendingPreference
{
    [Key]
    [Column("preference_id")]
    public int PreferenceId { get; set; }

    [Column("member_id")]
    public int MemberId { get; set; }

    [Column("resource_type")]
    [StringLength(10)]
    [Unicode(false)]
    public string? ResourceType { get; set; }

    [Column("topic")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Topic { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("LendingPreferences")]
    public virtual Member Member { get; set; } = null!;
}
