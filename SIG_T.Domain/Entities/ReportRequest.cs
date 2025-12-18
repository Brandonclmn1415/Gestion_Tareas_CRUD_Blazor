using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIG_T.Domain.Entities;

[Table("ReportRequest", Schema = "dbo")]
public class ReportRequest
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ReportType { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public string? Payload { get; set; }
}