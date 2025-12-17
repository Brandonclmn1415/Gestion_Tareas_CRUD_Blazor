using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIG_T.Shared.Domain.Entities;

/// <summary>
/// Activity log entity for audit trail
/// </summary>
[Table("RegistroDeActividad", Schema = "dbo")]
public class RegistroDeActividad
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TareaId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Accion { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(100)]
    public string Usuario { get; set; } = string.Empty;
}