using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib_Staff_Management_Lib.Data;

public partial class Location
{
    [Column("ID")]
    public int Id { get; set; }

    [Key]
    [Column("locationID")]
    public Guid LocationId { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public byte[] Name { get; set; } = null!;

    [Column("is_city")]
    public bool IsCity { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }
}
