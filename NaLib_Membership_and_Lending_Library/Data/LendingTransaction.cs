using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Membership_and_Lending_Lib.Data;

[Table("lending_transactions")]
[Index("MemberId", Name = "idx_lending_transactions_member_id")]
[Index("ResourceId", Name = "idx_lending_transactions_resource_id")]
public partial class LendingTransaction
{
    [Key]
    [Column("transaction_id")]
    public int TransactionId { get; set; }

    [Column("member_id")]
    public int MemberId { get; set; }

    [Column("resource_id")]
    public int ResourceId { get; set; }

    [Column("checkout_date")]
    public DateOnly CheckoutDate { get; set; }

    [Column("due_date")]
    public DateOnly DueDate { get; set; }

    [Column("return_date")]
    public DateOnly? ReturnDate { get; set; }

    [Column("status")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Status { get; set; }

    [InverseProperty("Transaction")]
    public virtual ICollection<DamageAndLoss> DamageAndLosses { get; set; } = new List<DamageAndLoss>();

    [ForeignKey("MemberId")]
    [InverseProperty("LendingTransactions")]
    public virtual Member Member { get; set; } = null!;

    [ForeignKey("ResourceId")]
    [InverseProperty("LendingTransactions")]
    public virtual Resource Resource { get; set; } = null!;
}
