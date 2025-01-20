using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Membership_and_Lending_Lib.Data;

[Table("resources")]
[Index("Title", Name = "idx_resources_title")]
public partial class Resource
{
    [Key]
    [Column("resource_id")]
    public int ResourceId { get; set; }

    [Column("title")]
    [StringLength(255)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column("author")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Author { get; set; }

    [Column("publisher")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Publisher { get; set; }

    [Column("publication_date")]
    public DateOnly? PublicationDate { get; set; }

    [Column("type")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Type { get; set; }

    [InverseProperty("Resource")]
    public virtual ICollection<LendingTransaction> LendingTransactions { get; set; } = new List<LendingTransaction>();
}
