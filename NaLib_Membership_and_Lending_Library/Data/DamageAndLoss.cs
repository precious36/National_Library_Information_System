using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Membership_and_Lending_Lib.Data;

[Table("damage_and_loss")]
public partial class DamageAndLoss
{
    [Key]
    [Column("damage_id")]
    public int DamageId { get; set; }

    [Column("transaction_id")]
    public int TransactionId { get; set; }

    [Column("damage_type")]
    [StringLength(10)]
    [Unicode(false)]
    public string? DamageType { get; set; }

    [Column("replacement_cost", TypeName = "decimal(10, 2)")]
    public decimal? ReplacementCost { get; set; }

    [ForeignKey("TransactionId")]
    [InverseProperty("DamageAndLosses")]
    public virtual LendingTransaction Transaction { get; set; } = null!;
}
